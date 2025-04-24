using EduPro.Models;
using EduPro.Models.ViewModels;
using EduPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduPro.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IUserCourseService _userCourseService;
        private readonly IErrorLogService _errorLogService;

        public PaymentController(
            ICartService cartService,
            IUserCourseService userCourseService,
            IErrorLogService errorLogService)
        {
            _cartService = cartService;
            _userCourseService = userCourseService;
            _errorLogService = errorLogService;
        }

        public async Task<IActionResult> Checkout()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var cartItems = await _cartService.GetCartItemsAsync(userId);

                if (cartItems.Count == 0)
                {
                    TempData["ErrorMessage"] = "Your cart is empty.";
                    return RedirectToAction("Index", "Cart");
                }

                var viewModel = new CheckoutViewModel
                {
                    CartItems = cartItems
                };

                viewModel.CalculateTotalAmount();

                return View("~/Views/EduPro/payment.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Payment", "Checkout", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "An error occurred during checkout. Please try again.";
                return RedirectToAction("Index", "Cart");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var cartItems = await _cartService.GetCartItemsAsync(userId);

                if (cartItems.Count == 0)
                {
                    TempData["ErrorMessage"] = "Your cart is empty.";
                    return RedirectToAction("Index", "Cart");
                }

               
                foreach (var item in cartItems)
                {
                    await _userCourseService.EnrollUserInCourseAsync(userId, item.CourseId);
                }

                await _cartService.ClearCartAsync(userId);

                TempData["SuccessMessage"] = "Payment successful! You are now enrolled in the courses.";
                return RedirectToAction("MyCourses", "Courses");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Payment", "ProcessPayment", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "Payment processing failed. Please try again.";
                return RedirectToAction("Checkout");
            }
        }
    }
    
}