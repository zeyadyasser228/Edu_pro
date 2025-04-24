using EduPro.Models;
using EduPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduPro.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IUserCourseService _userCourseService;
        private readonly ICourseService _courseService;
        private readonly IErrorLogService _errorLogService;

        public CoursesController(
            IUserCourseService userCourseService,
            ICourseService courseService,
            IErrorLogService errorLogService)
        {
            _userCourseService = userCourseService;
            _courseService = courseService;
            _errorLogService = errorLogService;
        }

        [Authorize]
        public async Task<IActionResult> MyCourses()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userCourses = await _userCourseService.GetUserCoursesAsync(userId);

                return View("~/Views/EduPro/MyCourses.cshtml", userCourses);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Courses", "MyCourses", User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View("Error");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProgress(int courseId, decimal completionPercentage)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _userCourseService.UpdateCourseProgressAsync(userId, courseId, completionPercentage);

                TempData["SuccessMessage"] = "Progress updated successfully!";
                return RedirectToAction("MyCourses");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Courses", "UpdateProgress", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "Failed to update progress. Please try again.";
                return RedirectToAction("MyCourses");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MarkAsCompleted(int courseId)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _userCourseService.MarkCourseAsCompletedAsync(userId, courseId);

                TempData["SuccessMessage"] = "Course marked as completed!";
                return RedirectToAction("MyCourses");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex, "Courses", "MarkAsCompleted", User.FindFirstValue(ClaimTypes.NameIdentifier));
                TempData["ErrorMessage"] = "Failed to mark course as completed. Please try again.";
                return RedirectToAction("MyCourses");
            }
        }
    }
}