using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduPro.Models
{
    public class UserModel
    {
        [Key] // primary key 
        public int Id { get; set; }

        [Required(ErrorMessage ="FullName or Username is Requird")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is Requird")]
        [EmailAddress] // @gmail.com    
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        public string ProfilePhotoPath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // sa3a al computer

        public DateTime? LastLoginAt { get; set; } // login at 

        public bool IsActive { get; set; } = true; // open to his account 
        
        public string ?VerificationCode { get; set; } 

        public DateTime ?VerificationCodeExpiry { get; set; }
    }
}