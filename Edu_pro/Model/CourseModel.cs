
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduPro.Models
{
    public class CourseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(20)]
        public string Level { get; set; } // Beginner, Intermediate, Advanced

        [Required]
        [StringLength(50)]
        public string Category { get; set; } // Programming, Design, Marketing, Business, Data Science .

        [Required]
        public int DurationInWeeks { get; set; }

        [Required]
        public int LessonsCount { get; set; }

        public int StudentsEnrolled { get; set; } = 0;

        [Range(0, 5)] // 
        public decimal Rating { get; set; } = 0;

        public int RatingCount { get; set; } = 0;

        [StringLength(255)]
        public string ExternalUrl { get; set; } // Optional

        public bool IsFeatured { get; set; } = false; // 
         
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }


    }
}