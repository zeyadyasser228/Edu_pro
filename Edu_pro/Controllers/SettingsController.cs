using Edu_pro.Model.ViewModels;
using EduPro.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Edu_pro.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View("~/Views/EduPro/Settings.cshtml");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View("~/Views/EduPro/Settings.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/EduPro/Settings.cshtml", model);

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["Error"] = "You must be logged in to change your password.";
                    return RedirectToAction("Login", "Auth");
                }

                var user = await _context.Users.FindAsync(int.Parse(userId));

                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return View("~/Views/EduPro/Settings.cshtml", model);
                }

                if (!VerifyPasswordHash(model.CurrentPassword, user.Password))
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                    return View("~/Views/EduPro/Settings.cshtml", model);
                }

                user.Password = HashPassword(model.NewPassword);
                _context.Update(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Password updated successfully!";
                return RedirectToAction("Index", "EduPro"); 
            }
            catch
            {
                TempData["Error"] = "An error occurred while changing the password. Please try again later.";
                return View("~/Views/EduPro/Settings.cshtml", model);
            }
        }


        private bool VerifyPasswordHash(string password, string storedHash)
        {
            string computedHash = HashPassword(password);
            return computedHash.Equals(storedHash);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}