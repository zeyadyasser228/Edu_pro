// Services/IUserCourseService.cs
using EduPro.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduPro.Services
{
    public interface IUserCourseService
    {
        Task<List<UserCourseModel>> GetUserCoursesAsync(int userId);
        Task<UserCourseModel> EnrollUserInCourseAsync(int userId, int courseId);
        Task<bool> IsUserEnrolledAsync(int userId, int courseId);
        Task<bool> UpdateCourseProgressAsync(int userId, int courseId, decimal completionPercentage);
        Task<bool> MarkCourseAsCompletedAsync(int userId, int courseId);
    }
}