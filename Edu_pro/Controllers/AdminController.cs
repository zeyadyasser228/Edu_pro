using EduPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EduPro.Data;
using EduPro.Filter;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Edu_pro.Models;

namespace Edu_pro.Controllers
{


    //[Authorize(Roles ="Admin")]

    [AdminAuthorize]

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm, string category, string featured, int page = 1)
        {
            const int pageSize = 10;

            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Title.Contains(searchTerm) || c.Description.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(c => c.Category == category);
            }

            if (!string.IsNullOrEmpty(featured))
            {
                var isFeatured = featured.ToLower() == "true";
                query = query.Where(c => c.IsFeatured == isFeatured);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Ensure page is within valid range
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            // Get paginated results
            var courses = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Pass pagination info to the view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            return View("~/Views/EduPro/Admin.cshtml", courses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, CourseModel course, IFormFile imageFile)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    // Handle image upload if a new image is provided
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "courses");

                        // Create directory if it doesn't exist
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        // Create a unique filename
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Update the course with the new image URL
                        course.ImageUrl = $"/uploads/courses/{fileName}";
                    }
                    else
                    {
                        // Keep the existing image if no new one is uploaded
                        var existingCourse = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                        if (existingCourse != null)
                        {
                            course.ImageUrl = existingCourse.ImageUrl;
                        }
                    }

                    course.UpdatedAt = DateTime.UtcNow;
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var courses = await _context.Courses.OrderByDescending(c => c.CreatedAt).Take(10).ToListAsync();
            ViewBag.ErrorMessage = "Failed to update course. Please check the form and try again.";
            return View("~/Views/EduPro/Admin.cshtml", courses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(CourseModel course, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "courses");

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // Create a unique filename
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    course.ImageUrl = $"/uploads/courses/{fileName}";
                }

                course.CreatedAt = DateTime.UtcNow;
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we get here, something failed, redisplay form
            var courses = await _context.Courses.OrderByDescending(c => c.CreatedAt).Take(10).ToListAsync();
            ViewBag.ErrorMessage = "Failed to add course. Please check the form and try again.";
            return View("~/Views/EduPro/Admin.cshtml", courses);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseData(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Json(course);
        }

    

     
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
        public IActionResult TestDatabase()
        {
            try
            {
                bool canConnect = _context.Database.CanConnect();

                int courseCount = _context.Courses.Count();

                // Try to add a test course
                var testCourse = new CourseModel
                {
                    Title = "Test Course " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Description = "Test Description",
                    Category = "Test",
                    Price = 9.99m,
                    Level = "Beginner",
                    ImageUrl = "/images/test.jpg",
                    DurationInWeeks = 1,
                    LessonsCount = 1,
                    CreatedAt = DateTime.UtcNow,
                    ExternalUrl="www.route.com"
                };

                _context.Courses.Add(testCourse);
                _context.SaveChanges();

                return Content($"Database connection: {canConnect}, Course count: {courseCount}, Test course added with ID: {testCourse.Id}");
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}, Inner: {ex.InnerException?.Message}");
            }
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Analytics()
        {
            try
            {
                var topCourses = await _context.Courses
                    .OrderByDescending(c => c.StudentsEnrolled)
                    .Take(5)
                    .Select(c => new CourseAnalytics
                    {
                        Id = c.Id,
                        CourseName = c.Title,
                        EnrollmentCount = c.StudentsEnrolled,
                        CompletionRate = GetStaticCompletionRate(c.StudentsEnrolled),
                        AverageRating = c.Rating,
                        Revenue = c.Price * c.StudentsEnrolled,
                        LastUpdated = c.UpdatedAt ?? c.CreatedAt
                    })
                    .ToListAsync();

                var totalStudents = await _context.Courses.SumAsync(c => (int?)c.StudentsEnrolled) ?? 0;
                var activeEnrollments = await _context.Courses
                    .Where(c => c.CreatedAt >= DateTime.Now.AddMonths(-1))
                    .SumAsync(c => (int?)c.StudentsEnrolled) ?? 0;
                var totalRevenue = await _context.Courses
                    .SumAsync(c => (decimal?)(c.Price * c.StudentsEnrolled)) ?? 0;

                var categoryEnrollments = await _context.Courses
                    .GroupBy(c => c.Category)
                    .Select(g => new { g.Key, Count = g.Sum(c => c.StudentsEnrolled) })
                    .ToDictionaryAsync(x => x.Key ?? "Uncategorized", x => x.Count);

                var recentCourses = await _context.Courses
                    .Where(c => c.CreatedAt >= DateTime.Now.AddMonths(-6))
                    .ToListAsync();

                var enrollmentsByMonth = Enumerable.Range(0, 6)
                    .Select(i => DateTime.Now.AddMonths(-i))
                    .Reverse()
                    .ToDictionary(
                        date => date.ToString("MMMM"),
                        date => recentCourses
                            .Where(c => c.CreatedAt.Month == date.Month && c.CreatedAt.Year == date.Year)
                            .Sum(c => c.StudentsEnrolled)
                    );

                var analytics = new DashboardAnalytics
                {
                    TopCourses = topCourses,
                    TotalStudents = totalStudents,
                    ActiveEnrollments = activeEnrollments,
                    TotalRevenue = totalRevenue,
                    EnrollmentsByCategory = categoryEnrollments,
                    EnrollmentsByMonth = enrollmentsByMonth
                };

                return View(analytics);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Analytics error: {ex.Message}");

                var fallback = new DashboardAnalytics
                {
                    TopCourses = new List<CourseAnalytics>(),
                    TotalStudents = 0,
                    ActiveEnrollments = 0,
                    TotalRevenue = 0,
                    EnrollmentsByCategory = new Dictionary<string, int> { ["No Data"] = 0 },
                    EnrollmentsByMonth = Enumerable.Range(0, 6)
                        .Select(i => DateTime.Now.AddMonths(-i).ToString("MMMM"))
                        .ToDictionary(m => m, m => 0)
                };

                return View(fallback);
            }
        }


        [HttpGet]
        public async Task<JsonResult> GetAnalyticsData()
        {
            try
            {
                var courses = await _context.Courses.ToListAsync();
                
                if (!courses.Any())
                {
                    return Json(new
                    {
                        enrollmentTrend = new[] { new { month = DateTime.Now.ToString("MMM"), count = 0 } },
                        topCourses = new object[0],
                        revenueByCategory = new object[0],
                        totalRevenue = 0,
                        totalStudents = 0,
                        activeEnrollments = 0,
                        averageRating = 0.0
                    });
                }

                var analyticsData = new
                {
                    enrollmentTrend = await _context.Courses
                        .Where(c => c.CreatedAt >= DateTime.Now.AddMonths(-6))
                        .GroupBy(c => c.CreatedAt.ToString("MMM"))
                        .Select(g => new { month = g.Key, count = g.Sum(c => c.StudentsEnrolled) })
                        .ToListAsync(),

                    topCourses = await _context.Courses
                        .OrderByDescending(c => c.StudentsEnrolled)
                        .Take(5)
                        .Select(c => new 
                        { 
                            name = c.Title ?? "Untitled Course", 
                            count = c.StudentsEnrolled,
                            rating = c.Rating,
                            revenue = c.Price * c.StudentsEnrolled,
                            completionRate = GetStaticCompletionRate(c.StudentsEnrolled)
                        })
                        .ToListAsync(),

                    revenueByCategory = await _context.Courses
                        .GroupBy(c => c.Category ?? "Uncategorized")
                        .Select(g => new 
                        { 
                            category = g.Key, 
                            revenue = g.Sum(c => c.Price * c.StudentsEnrolled) 
                        })
                        .ToListAsync(),

                    totalRevenue = await _context.Courses.SumAsync(c => (decimal?)c.Price * c.StudentsEnrolled) ?? 0,
                    totalStudents = await _context.Courses.SumAsync(c => (int?)c.StudentsEnrolled) ?? 0,
                    activeEnrollments = await _context.Courses
                        .Where(c => c.CreatedAt >= DateTime.Now.AddMonths(-1))
                        .SumAsync(c => (int?)c.StudentsEnrolled) ?? 0,
                    averageRating = await _context.Courses
                        .Where(c => c.Rating > 0)
                        .Select(c => (double?)c.Rating)
                        .DefaultIfEmpty(0)
                        .AverageAsync() ?? 0
                };

                return Json(analyticsData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAnalyticsData: {ex.Message}");
                return Json(new { error = "Failed to load analytics data" });
            }
        }

        // Static method to calculate completion rate
        private static decimal GetStaticCompletionRate(int enrollments)
        {
            if (enrollments <= 0) return 0;
            
            decimal baseRate = 70.0m;
            decimal enrollmentFactor = Math.Min(enrollments / 100.0m, 0.2m);
            return Math.Min(baseRate + (enrollmentFactor * 100), 95.0m);
        }
    }

}