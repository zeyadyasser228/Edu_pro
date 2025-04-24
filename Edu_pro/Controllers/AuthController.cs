using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using EduPro.Models;
using EduPro.Models.ViewModels;
using EduPro.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EduPro.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IErrorLogService _errorLogService;
        private readonly IWebHostEnvironment _environment; // 3la4an al profile photo uplaod 
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailService _emailService;


        public AuthController(
            IUserService userService,
            IErrorLogService errorLogService,
            IWebHostEnvironment environment,
            ILogger<AuthController> logger,
            IEmailService emailService)
             
        {
            _userService = userService;
            _errorLogService = errorLogService;
            _environment = environment;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Signup()
        {
            // by4of lw al user 3amel login , yro7 ll edupro page 
            if (User.Identity.IsAuthenticated)
            {
                return View("~/Views/EduPro/edupro.cshtml");
            }

            return View("~/Views/EduPro/signup.cshtml", new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/EduPro/signup.cshtml", model);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                    return View("~/Views/EduPro/signup.cshtml", model);
                }

                if (await _userService.EmailExistsAsync(model.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered");
                    return View("~/Views/EduPro/signup.cshtml", model);
                }

                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "profiles");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    ModelState.AddModelError("Photo", "Profile photo is required");
                    return View("~/Views/EduPro/signup.cshtml", model);
                }

                var user = new UserModel
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    ProfilePhotoPath = uniqueFileName,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _userService.CreateUserAsync(user);

                _logger.LogInformation($"User {model.Email} registered successfully");

                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return View("~/Views/EduPro/signup.cshtml", new LoginViewModel { Email = model.Email });
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Auth", "Signup", null, model.Email);
                ViewData["SignupError"] = "An error occurred during registration. Please try again.";
                return View("~/Views/EduPro/signup.cshtml", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/EduPro/signup.cshtml", model);
                }

                var user = await _userService.AuthenticateAsync(model.Email, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View("~/Views/EduPro/signup.cshtml", model);
                }

                await _userService.UpdateLastLoginAsync(user.Id);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email)
        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation($"User {model.Email} logged in successfully");



                // b4of lw fe courses kant fel cart 
                if (TempData["PendingCourseId"] != null)
                {
                    int courseId = (int)TempData["PendingCourseId"];
             
                    return RedirectToAction("AddToCart", "Cart", new { courseId });
                }

                return RedirectToAction("Index", "EduPro");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Auth", "Login", null, model.Email);
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
                return View("~/Views/EduPro/signup.cshtml", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return RedirectToAction("Index", "EduPro");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Auth", "Logout", null, User?.FindFirstValue(ClaimTypes.Email));

                return RedirectToAction("Index", "EduPro");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationCode(string email)
        {
            try
            {
                var userExists = await _userService.EmailExistsAsync(email);
                if (!userExists)
                {
                    return Json(new { success = false, message = "Email not found." });
                }

                var code = await _userService.GeneratePasswordResetCodeAsync(email);
                if (code == null)
                {
                    return Json(new { success = false, message = "Failed to generate verification code." });
                }

                try
                {
                    string subject = "Password Reset Verification Code";
                    string message = $@"
                <html>
                <body>
                    <h2>Password Reset</h2>
                    <p>You requested a password reset for your EduPro account.</p>
                    <p>Your verification code is: <strong>{code}</strong></p>
                    <p>This code will expire in 5 minutes.</p>
                    <p>If you didn't request this, please ignore this email.</p>
                </body>
                </html>";

                    await _emailService.SendEmailAsync(email, subject, message);

                    return Json(new { success = true });
                }
                catch (Exception emailEx)
                {
                    await _errorLogService.LogErrorAsync(emailEx, "Auth", "SendVerificationCode - Email Sending", null, email);
                    return Json(new { success = false, message = "Failed to send email. Please try again later." });
                }
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Auth", "SendVerificationCode", null, email);
                return Json(new { success = false, message = "An error occurred. Please try again." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(string email, string code)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
                {
                    return Json(new { success = false, message = "Email and verification code are required." });
                }

                _logger.LogInformation($"Verifying code: Email={email}, Code={code}");

                var isValid = await _userService.VerifyResetCodeAsync(email, code);

                if (!isValid)
                {
                    _logger.LogWarning($"Invalid or expired code for email: {email}");
                    return Json(new { success = false, message = "Invalid or expired verification code." });
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Auth", "VerifyCode", null, email);
                return Json(new { success = false, message = "An error occurred. Please try again." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email, string newPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword))
                {
                    return Json(new { success = false, message = "Email and new password are required." });
                }

                _logger.LogInformation($"Resetting password for email: {email}");

                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                await _userService.ResetPasswordAsync(email, newPassword);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Auth", "ResetPassword", null, email);
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }


    }



}