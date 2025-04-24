
using EduPro.Data;
using EduPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduPro.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context; // Entity Framwork 
        private readonly ILogger<CourseService> _logger; // Logger class CourseService 

        public CourseService(ApplicationDbContext context, ILogger<CourseService> logger)
        {
            _context = context;
            _logger = logger;
        }
        // Fetch to all courses from Database
        public async Task<List<CourseModel>> GetAllCoursesAsync()
        {
            _logger.LogInformation("Getting all courses from database");
            var courses = await _context.Courses.OrderBy(c => c.Title).ToListAsync();
            _logger.LogInformation($"Retrieved {courses.Count} courses from database");
            return courses;
        }
        // Featurred : courses مميزه 
        public async Task<List<CourseModel>> GetFeaturedCoursesAsync(int count = 3)
        {
            return await _context.Courses
                .Where(c => c.IsFeatured)
                .OrderByDescending(c => c.Rating)
                .Take(count)
                .ToListAsync();
        }
        //
        public async Task<CourseModel> GetCourseByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<List<CourseModel>> GetCoursesByCategoryAsync(string category)
        {
            return await _context.Courses
                .Where(c => c.Category == category)
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<List<CourseModel>> GetCoursesByLevelAsync(string level)
        {
            return await _context.Courses
                .Where(c => c.Level == level)
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<List<CourseModel>> SearchCoursesAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetAllCoursesAsync();

            return await _context.Courses 
                .Where(c => c.Title.Contains(searchTerm) || c.Description.Contains(searchTerm))
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<List<CourseModel>> FilterCoursesAsync(string category = null, string level = null, string duration = null)
        {
            var query = _context.Courses.AsQueryable(); // Be ready to put Query in the Query Attrubo

            if (!string.IsNullOrEmpty(category) && category != "all")
            {
                query = query.Where(c => c.Category == category);
            }

            if (!string.IsNullOrEmpty(level) && level != "all")
            {
                query = query.Where(c => c.Level == level);
            }

            if (!string.IsNullOrEmpty(duration) && duration != "all")
            {
                switch (duration)
                {
                    case "short":
                        query = query.Where(c => c.DurationInWeeks >= 1 && c.DurationInWeeks <= 4);
                        break;
                    case "medium":
                        query = query.Where(c => c.DurationInWeeks >= 5 && c.DurationInWeeks <= 8);
                        break;
                    case "long":
                        query = query.Where(c => c.DurationInWeeks >= 9);
                        break;
                }
            }

            return await query.OrderBy(c => c.Title).ToListAsync();
        }

        public async Task<List<string>> GetAllCategoriesAsync()
        {
            return await _context.Courses
                .Select(c => c.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<List<string>> GetAllLevelsAsync()
        {
            return await _context.Courses
                .Select(c => c.Level)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
        // 
        public async Task<(List<CourseModel> Courses, int TotalPages, int TotalItems)> GetPaginatedCoursesAsync(int page, int pageSize)
        {
            var query = _context.Courses.AsQueryable(); // Be ready 
            // TOTAL AL COURSES
            var totalItems = await query.CountAsync();

            //  total pages
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Ensure page is within valid range
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var courses = await query
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (courses, totalPages, totalItems);
        }
    }
}