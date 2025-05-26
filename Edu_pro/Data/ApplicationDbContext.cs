using EduPro.Models;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; } // user on the website 
        public DbSet<ErrorLog> ErrorLogs { get; set; } // error faces the website 
        public DbSet<CourseModel> Courses { get; set; } // Courses aly 3ndna 
        public DbSet<CartItemModel> CartItems { get; set; } // Each user have cart (User & Cart )
        public DbSet<UserCourseModel> UserCourses { get; set; } // user cours (User & Courses )

        // 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // user entity hna index 3la al email 
            // Fluent APi 
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique(); // cannot Be Duplicated 
        }
    }
}