// Services/ICourseService.cs
using EduPro.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduPro.Services
{
    public interface ICourseService
    {

        // fetch courses fel page 
        Task<List<CourseModel>> GetAllCoursesAsync();

        Task<List<CourseModel>> GetFeaturedCoursesAsync(int count = 3);
        Task<CourseModel> GetCourseByIdAsync(int id);
        Task<List<CourseModel>> GetCoursesByCategoryAsync(string category);
        Task<List<CourseModel>> GetCoursesByLevelAsync(string level);
        Task<List<CourseModel>> SearchCoursesAsync(string searchTerm);
        Task<List<CourseModel>> FilterCoursesAsync(string category = null, string level = null, string duration = null);
        Task<List<string>> GetAllCategoriesAsync();
        Task<List<string>> GetAllLevelsAsync();
        Task<(List<CourseModel> Courses, int TotalPages, int TotalItems)> GetPaginatedCoursesAsync(int page, int pageSize);

    }
}