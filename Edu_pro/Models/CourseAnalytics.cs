using System;
using System.Collections.Generic;

namespace Edu_pro.Models
{
    public class CourseAnalytics
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int EnrollmentCount { get; set; }
        public decimal CompletionRate { get; set; }
        public decimal AverageRating { get; set; }
        public decimal Revenue { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class DashboardAnalytics
    {
        public List<CourseAnalytics> TopCourses { get; set; }
        public int TotalStudents { get; set; }
        public int ActiveEnrollments { get; set; }
        public decimal TotalRevenue { get; set; }
        public Dictionary<string, int> EnrollmentsByCategory { get; set; }
        public Dictionary<string, int> EnrollmentsByMonth { get; set; }
    }
} 