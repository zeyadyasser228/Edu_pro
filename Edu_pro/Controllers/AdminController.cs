using EduPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EduPro.Data;
using EduPro.Filer;
using Microsoft.AspNetCore.Authorization;

namespace Edu_pro.Controllers
{

   

    [AdminAuthorize]

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method 1: Index method with filtering and pagination
        public async Task<IActionResult> Index(string searchTerm, string category, string featured, int page = 1)
        {
            const int pageSize = 10; // Number of courses per page

            var query = _context.Courses.AsQueryable();

            // Apply filters if provided
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

            // Calculate pagination
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

        // Method 2: Edit course POST
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

            // If we get here, something failed, redisplay form
            var courses = await _context.Courses.OrderByDescending(c => c.CreatedAt).Take(10).ToListAsync();
            ViewBag.ErrorMessage = "Failed to update course. Please check the form and try again.";
            return View("~/Views/EduPro/Admin.cshtml", courses);
        }

        // Method 2: Delete course
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

        // Method 2: Add course POST
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

        // Method 3: Get course data for the edit form
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

        //// Method 4: Bulk delete courses
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> BulkDeleteCourses(int[] ids)
        //{
        //    if (ids == null || ids.Length == 0)
        //    {
        //        return BadRequest("No courses selected");
        //    }

        //    var courses = await _context.Courses.Where(c => ids.Contains(c.Id)).ToListAsync();
        //    _context.Courses.RemoveRange(courses);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        //// Method 5: Bulk feature/unfeature courses
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> BulkFeatureCourses(int[] ids, bool featured)
        //{
        //    if (ids == null || ids.Length == 0)
        //    {
        //        return BadRequest("No courses selected");
        //    }

        //    var courses = await _context.Courses.Where(c => ids.Contains(c.Id)).ToListAsync();
        //    foreach (var course in courses)
        //    {
        //        course.IsFeatured = featured;
        //        course.UpdatedAt = DateTime.UtcNow;
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        // Helper method to check if a course exists
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
        public IActionResult TestDatabase()
        {
            try
            {
                bool canConnect = _context.Database.CanConnect();

                // Try to get a count
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
    }

}