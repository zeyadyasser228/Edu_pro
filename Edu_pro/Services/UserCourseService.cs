using EduPro.Data;
using EduPro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduPro.Services
{
    public class UserCourseService : IUserCourseService
    {
        private readonly ApplicationDbContext _context; // Entity Framwork 
        private readonly ICourseService _courseService; // 

        public UserCourseService(ApplicationDbContext context, ICourseService courseService)
        {
            _context = context;
            _courseService = courseService;
        }
        // Fetch all enrolled Courses bta3at al User b el USer ID 
        public async Task<List<UserCourseModel>> GetUserCoursesAsync(int userId)
        {
            return await _context.UserCourses
                .Where(uc => uc.UserId == userId) // Find User id 
                .Include(uc => uc.Course) // Load Them 
                .ToListAsync(); // Save them to List 
        }

        // USer enroll in Courses
        public async Task<UserCourseModel> EnrollUserInCourseAsync(int userId, int courseId)
        {
            // By4of al COurse Mwgod wla l2a 
            var course = await _courseService.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                throw new ArgumentException($"Course with ID {courseId} not found");
            }
            // B4of al user asln 3amel enroll wla l2a 
            var existingEnrollment = await _context.UserCourses
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

            if (existingEnrollment != null)
            {
                return existingEnrollment;
            }
            // kda hwa awl mra 
            var enrollment = new UserCourseModel
            {
                UserId = userId,
                CourseId = courseId,
                EnrollmentDate = DateTime.UtcNow,
                IsCompleted = false,
                CompletionPercentage = 0
            };

            await _context.UserCourses.AddAsync(enrollment);

            course.StudentsEnrolled += 1;// update COurses table 
            _context.Courses.Update(course);  // make update Query 

            await _context.SaveChangesAsync(); 

            return enrollment;
        }

        public async Task<bool> IsUserEnrolledAsync(int userId, int courseId)
        {
            return await _context.UserCourses
                .AnyAsync(uc => uc.UserId == userId && uc.CourseId == courseId);
        }

        public async Task<bool> UpdateCourseProgressAsync(int userId, int courseId, decimal completionPercentage)
        {
            var enrollment = await _context.UserCourses
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

            if (enrollment == null)
            {
                return false;
            }

            enrollment.CompletionPercentage = completionPercentage;

            if (completionPercentage >= 100)
            {
                enrollment.IsCompleted = true;
            }

            _context.UserCourses.Update(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkCourseAsCompletedAsync(int userId, int courseId)
        {
            var enrollment = await _context.UserCourses
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

            if (enrollment == null)
            {
                return false;
            }

            enrollment.IsCompleted = true;
            enrollment.CompletionPercentage = 100;

            _context.UserCourses.Update(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}