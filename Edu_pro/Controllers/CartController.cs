using EduPro.Models;
using EduPro.Models.ViewModels;
using EduPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduPro.Controllers
{
    public class CartController : Controller  
    {
        private readonly ICartService _cartService;
        private readonly ICourseService _courseService;
        private readonly IErrorLogService _errorLogService;

        public CartController(
            ICartService cartService,
            ICourseService courseService,
            IErrorLogService errorLogService)
        {
            _cartService = cartService;
            _courseService = courseService;
            _errorLogService = errorLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    TempData["InfoMessage"] = "Please log in to view your cart.";
                    return View("~/Views/EduPro/Cart.cshtml", new CartViewModel());
                }

                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var cartItems = await _cartService.GetCartItemsAsync(userId);

                var viewModel = new CartViewModel
                {
                    Items = new List<CartItemViewModel>()
                };

                foreach (var item in cartItems)
                {
                    viewModel.Items.Add(new CartItemViewModel
                    {
                        Id = item.Id,
                        CourseId = item.CourseId,
                        Title = item.Course.Title,
                        Description = item.Course.Description,
                        ImageUrl = item.Course.ImageUrl,
                        Price = item.Course.Price,
                        Level = item.Course.Level,
                        DurationInWeeks = item.Course.DurationInWeeks,
                        LessonsCount = item.Course.LessonsCount,
                        AddedAt = item.AddedAt
                    });
                }

                return View("~/Views/EduPro/Cart.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Cart", "Index", User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View("~/Views/EduPro/Cart.cshtml", new CartViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int courseId)
        {
            try
            {
                // Check if the user is authenticated
                if (!User.Identity.IsAuthenticated)
                {
                    // Store the courseId in TempData so we can add it to cart after login
                    TempData["PendingCourseId"] = courseId;
                    TempData["InfoMessage"] = "Please log in to add courses to your cart.";

                    // Redirect to login page
                    return RedirectToAction("Signup", "Auth");
                }

                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _cartService.AddToCartAsync(userId, courseId);

                TempData["SuccessMessage"] = "Course added to cart successfully!";
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Cart", "AddToCart", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "Failed to add course to cart. Please try again.";
                return RedirectToAction("Details", "Course", new { id = courseId });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _cartService.RemoveFromCartAsync(userId, cartItemId);

                TempData["SuccessMessage"] = "Item removed from cart successfully!";
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Cart", "RemoveFromCart", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "Failed to remove item from cart. Please try again.";
                return RedirectToAction("Index", "Cart");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _cartService.ClearCartAsync(userId);

                TempData["SuccessMessage"] = "Cart cleared successfully!";
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Cart", "ClearCart", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "Failed to clear cart. Please try again.";
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}