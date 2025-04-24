using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduPro.Models
{
    public class UserCourseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        public bool IsCompleted { get; set; } = false;

        public decimal? CompletionPercentage { get; set; } = 0;

        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        [ForeignKey("CourseId")]
        public CourseModel Course { get; set; }
    }
}