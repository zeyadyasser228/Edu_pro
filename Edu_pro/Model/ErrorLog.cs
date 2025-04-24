using System;
using System.ComponentModel.DataAnnotations;

namespace EduPro.Models
{
    public class ErrorLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ErrorMessage { get; set; }

        [Required]
        public string StackTrace { get; set; } // path 

        [Required]
        public string ControllerName { get; set; }

        [Required]
        public string ActionName { get; set; }

        public string UserId { get; set; } // meen user is opened 

        public string UserEmail { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string AdditionalInfo { get; set; }

        [Required]
        public string ErrorSeverity { get; set; } = "Error"; 

        public string RequestPath { get; set; }

        public string RequestMethod { get; set; }

        public string UserAgent { get; set; }

        public string IpAddress { get; set; }
    }
}