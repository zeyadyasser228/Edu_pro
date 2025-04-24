using EduPro.Models;
using EduPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduPro.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
 
        private readonly IErrorLogService _errorLogService;
        private readonly ICartService _cartService;


        public CourseController(
             ICourseService courseService,
             ICartService cartService,
             IErrorLogService errorLogService)
        {
            _courseService = courseService;
            _cartService = cartService;
            _errorLogService = errorLogService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 6;
            var result = await _courseService.GetPaginatedCoursesAsync(page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = result.TotalPages;

            return View("~/Views/EduPro/course.cshtml", result.Courses);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound();

            return View("~/Views/EduPro/CourseDetails.cshtml", course);
        }

        public async Task<IActionResult> Category(string category, int page = 1)
        {
            if (string.IsNullOrEmpty(category))
                return RedirectToAction(nameof(Index));

            var courses = await _courseService.GetCoursesByCategoryAsync(category);

            int pageSize = 6;
            int totalItems = courses.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var paginatedCourses = courses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentCategory = category;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View("~/Views/EduPro/course.cshtml", paginatedCourses);
        }

        public async Task<IActionResult> Search(string searchTerm, int page = 1)
        {
            var courses = await _courseService.SearchCoursesAsync(searchTerm);

            int pageSize = 6;
            int totalItems = courses.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var paginatedCourses = courses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View("~/Views/EduPro/course.cshtml", paginatedCourses);
        }

        public async Task<IActionResult> Filter(string category, string level, string duration, int page = 1)
        {
            var courses = await _courseService.FilterCoursesAsync(category, level, duration);

            int pageSize = 6;
            int totalItems = courses.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var paginatedCourses = courses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Category = category;
            ViewBag.Level = level;
            ViewBag.Duration = duration;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View("~/Views/EduPro/course.cshtml", paginatedCourses);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _cartService.AddToCartAsync(userId, id);

                TempData["SuccessMessage"] = "Course added to cart successfully!";
                return RedirectToAction("Details", new { id });
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Course", "AddToCart", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "Failed to add course to cart. Please try again.";
                return RedirectToAction("Details", new { id });
            }
        }
    }
}