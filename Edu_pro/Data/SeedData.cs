using EduPro.Data;
using EduPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EduPro.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Courses.Any()) // Any : return true or false 
                {
                    return; 
                }

                context.Courses.AddRange(
                    // Programming Courses
                    new CourseModel
                    {
                        Title = "C# Programming Fundamentals",
                        Description = "Master the fundamentals of C# programming language. This comprehensive course covers variables, data types, control structures, object-oriented programming concepts, and practical application development. You'll build real-world projects and gain the skills needed for modern software development with Microsoft's powerful language.",
                        ImageUrl = "/images/courses/C#.jpg", 
                        Price = 49.99m,
                        Level = "Beginner",
                        Category = "Programming",
                        DurationInWeeks = 6,
                        LessonsCount = 42,
                        StudentsEnrolled = 1245,
                        Rating = 4.7m,
                        RatingCount = 328,
                        ExternalUrl = "https://www.example.com/csharp-course",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now.AddMonths(-6)
                    },
                    new CourseModel
                    {
                        Title = "ASP.NET MVC Web Development",
                        Description = "Learn how to build dynamic, scalable web applications using ASP.NET MVC. This course covers the Model-View-Controller architectural pattern, routing, controllers, views, models, Entity Framework for data access, authentication, authorization, and deployment. By the end, you'll be able to create professional web applications with clean, maintainable code.",
                        ImageUrl = "/images/courses/aspnet.jpg", 
                        Price = 69.99m,
                        Level = "Intermediate",
                        Category = "Programming",
                        DurationInWeeks = 8,
                        LessonsCount = 56,
                        StudentsEnrolled = 873,
                        Rating = 4.8m,
                        RatingCount = 214,
                        ExternalUrl = "https://www.example.com/aspnet-course",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now.AddMonths(-4)
                    },
                    new CourseModel
                    {
                        Title = "JavaScript for Modern Web Development",
                        Description = "Dive into modern JavaScript programming for web development. This course covers ES6+ features, DOM manipulation, asynchronous programming with Promises and async/await, working with APIs, and essential JavaScript patterns. You'll build interactive web applications and gain a solid foundation in one of the most important languages for web development.",
                        ImageUrl = "/images/courses/Js.jpg", 
                        Price = 59.99m,
                        Level = "Beginner",
                        Category = "Programming",
                        DurationInWeeks = 7,
                        LessonsCount = 48,
                        StudentsEnrolled = 2150,
                        Rating = 4.6m,
                        RatingCount = 412,
                        ExternalUrl = "https://www.example.com/javascript-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-8)
                    },
                    new CourseModel
                    {
                        Title = "Python Programming Mastery",
                        Description = "Become proficient in Python programming from the ground up. This course teaches Python syntax, data structures, functions, object-oriented programming, file handling, and libraries like NumPy and Pandas. You'll develop practical applications, automate tasks, and learn how Python is used in data science, web development, and automation.",
                        ImageUrl = "/images/courses/Python.jpg", 
                        Price = 54.99m,
                        Level = "All Levels",
                        Category = "Programming",
                        DurationInWeeks = 8,
                        LessonsCount = 62,
                        StudentsEnrolled = 3245,
                        Rating = 4.9m,
                        RatingCount = 567,
                        ExternalUrl = "https://www.example.com/python-course",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now.AddMonths(-3)
                    },
                    new CourseModel
                    {
                        Title = "React.js: Building Modern User Interfaces",
                        Description = "Learn to build dynamic user interfaces with React.js. This course covers components, props, state, hooks, context API, Redux for state management, routing with React Router, and integrating with APIs. You'll create responsive, interactive web applications with the most popular frontend library in the industry.",
                        ImageUrl = "/images/courses/React.jpg", 
                        Price = 64.99m,
                        Level = "Intermediate",
                        Category = "Programming",
                        DurationInWeeks = 6,
                        LessonsCount = 45,
                        StudentsEnrolled = 1876,
                        Rating = 4.8m,
                        RatingCount = 342,
                        ExternalUrl = "https://www.example.com/react-course",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now.AddMonths(-5)
                    },

                    // Design Courses
                    new CourseModel
                    {
                        Title = "UI/UX Design Principles",
                        Description = "Master the fundamentals of UI/UX design to create intuitive, beautiful user experiences. This course covers user research, wireframing, prototyping, visual design principles, typography, color theory, and usability testing. You'll build a professional portfolio and learn industry-standard tools like Figma and Adobe XD.",
                        ImageUrl = "/images/courses/ui&ux.jpg",
                        Price = 59.99m,
                        Level = "Beginner",
                        Category = "Design",
                        DurationInWeeks = 5,
                        LessonsCount = 38,
                        StudentsEnrolled = 1567,
                        Rating = 4.6m,
                        RatingCount = 412,
                        ExternalUrl = "https://www.example.com/uiux-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-7)
                    },
                    new CourseModel
                    {
                        Title = "Adobe Photoshop Masterclass",
                        Description = "Become proficient in Adobe Photoshop for professional image editing and manipulation. This comprehensive course covers photo retouching, compositing, digital painting, effects, layers, masks, and advanced techniques used by professionals. You'll work on real-world projects and develop skills for graphic design, web design, and photography.",
                        ImageUrl = "/images/courses/AdobePhotoshop.jpg",
                        Price = 64.99m,
                        Level = "All Levels",
                        Category = "Design",
                        DurationInWeeks = 10,
                        LessonsCount = 75,
                        StudentsEnrolled = 3421,
                        Rating = 4.8m,
                        RatingCount = 892,
                        ExternalUrl = "https://www.example.com/photoshop-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-9)
                    },
                    new CourseModel
                    {
                        Title = "Web Design: From Concept to Launch",
                        Description = "Learn the complete process of designing websites from initial concept to final launch. This course covers design principles, responsive design, HTML/CSS implementation, accessibility, performance optimization, and testing. You'll design and build a complete website and understand the full web design workflow.",
                        ImageUrl = "/images/courses/WebDesign.jpg",
                        Price = 69.99m,
                        Level = "Intermediate",
                        Category = "Design",
                        DurationInWeeks = 8,
                        LessonsCount = 56,
                        StudentsEnrolled = 1243,
                        Rating = 4.7m,
                        RatingCount = 325,
                        ExternalUrl = "https://www.example.com/web-design-course",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now.AddMonths(-6)
                    },
                    new CourseModel
                    {
                        Title = "Graphic Design Fundamentals",
                        Description = "Learn the core principles of graphic design including composition, color theory, typography, and visual hierarchy. This course teaches you how to create logos, branding materials, marketing collateral, and social media graphics. You'll develop a designer's eye and gain practical skills with industry-standard software.",
                        ImageUrl = "/images/courses/GraphicDesgign.jpg",
                        Price = 49.99m,
                        Level = "Beginner",
                        Category = "Design",
                        DurationInWeeks = 6,
                        LessonsCount = 42,
                        StudentsEnrolled = 2187,
                        Rating = 4.5m,
                        RatingCount = 456,
                        ExternalUrl = "https://www.example.com/graphic-design-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-8)
                    },

                    // Marketing Courses
                    new CourseModel
                    {
                        Title = "Digital Marketing Strategies",
                        Description = "Master comprehensive digital marketing strategies across multiple channels. This course covers SEO, SEM, social media marketing, content marketing, email campaigns, analytics, and conversion optimization. You'll develop complete marketing strategies to grow businesses online and measure campaign success.",
                        ImageUrl = "/images/courses/DigtialMarketing.jpg",
                        Price = 54.99m,
                        Level = "Intermediate",
                        Category = "Marketing",
                        DurationInWeeks = 6,
                        LessonsCount = 44,
                        StudentsEnrolled = 1832,
                        Rating = 4.5m,
                        RatingCount = 523,
                        ExternalUrl = "https://www.example.com/digital-marketing-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-5)
                    },
                    new CourseModel
                    {
                        Title = "Social Media Marketing Mastery",
                        Description = "Learn how to create effective social media marketing campaigns across platforms like Facebook, Instagram, Twitter, LinkedIn, and TikTok. This course covers content creation, community management, paid advertising, analytics, and developing a cohesive social media strategy that drives engagement and conversions.",
                        ImageUrl = "/images/courses/SoicalMedia.jpg",
                        Price = 49.99m,
                        Level = "All Levels",
                        Category = "Marketing",
                        DurationInWeeks = 5,
                        LessonsCount = 38,
                        StudentsEnrolled = 2456,
                        Rating = 4.6m,
                        RatingCount = 612,
                        ExternalUrl = "https://www.example.com/social-media-course",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now.AddMonths(-4)
                    },
                    new CourseModel
                    {
                        Title = "Content Marketing Strategy",
                        Description = "Master the art and science of content marketing to attract, engage, and convert audiences. This course covers content strategy, storytelling, blogging, video marketing, podcasting, content distribution, and measuring content ROI. You'll learn how to create valuable content that drives business results.",
                        ImageUrl = "/images/courses/ContentMarkting.jpg", 
                        Price = 59.99m,
                        Level = "Intermediate",
                        Category = "Marketing",
                        DurationInWeeks = 7,
                        LessonsCount = 46,
                        StudentsEnrolled = 1543,
                        Rating = 4.7m,
                        RatingCount = 387,
                        ExternalUrl = "https://www.example.com/content-marketing-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-7)
                    },

                    // Business Courses
                    new CourseModel
                    {
                        Title = "Business Analytics with Excel",
                        Description = "Learn powerful data analysis techniques using Microsoft Excel. This course covers data visualization, pivot tables, statistical analysis, dashboards, forecasting, and business intelligence. You'll develop skills to analyze business data, identify trends, and make data-driven decisions to improve performance.",
                        ImageUrl = "/images/courses/ExcelBussienes.jpg",
                        Price = 44.99m,
                        Level = "Beginner",
                        Category = "Business",
                        DurationInWeeks = 4,
                        LessonsCount = 32,
                        StudentsEnrolled = 2154,
                        Rating = 4.4m,
                        RatingCount = 678,
                        ExternalUrl = "https://www.example.com/excel-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-8)
                    },
                    new CourseModel
                    {
                        Title = "Project Management Professional",
                        Description = "Master project management methodologies, tools, and best practices. This course covers project planning, scheduling, budgeting, risk management, team leadership, and stakeholder communication. You'll learn both traditional and agile approaches to managing projects successfully from initiation to completion.",
                        ImageUrl = "/images/courses/ProjectManagement.jpg", 
                        Price = 79.99m,
                        Level = "Intermediate",
                        Category = "Business",
                        DurationInWeeks = 9,
                        LessonsCount = 64,
                        StudentsEnrolled = 1432,
                        Rating = 4.8m,
                        RatingCount = 356,
                        ExternalUrl = "https://www.example.com/project-management-course",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now.AddMonths(-3)
                    },
                    new CourseModel
                    {
                        Title = "Entrepreneurship: Launch Your Business",
                        Description = "Learn how to turn your business idea into reality. This comprehensive course covers business planning, market research, product development, funding strategies, marketing, sales, operations, and growth tactics. You'll develop a complete business plan and gain the knowledge to launch and grow your startup.",
                        ImageUrl = "/images/courses/Entrepreneurship.jpg",
                        Price = 69.99m,
                        Level = "All Levels",
                        Category = "Business",
                        DurationInWeeks = 8,
                        LessonsCount = 58,
                        StudentsEnrolled = 1876,
                        Rating = 4.7m,
                        RatingCount = 423,
                        ExternalUrl = "https://www.example.com/entrepreneurship-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-6)
                    },

                    // Data Science Courses
                    new CourseModel
                    {
                        Title = "Data Science Fundamentals",
                        Description = "Start your journey into data science with this comprehensive introduction. This course covers statistics, data analysis, Python programming, data visualization, machine learning basics, and practical data science workflows. You'll work with real datasets and build a foundation for a career in this high-demand field.",
                        ImageUrl = "/images/courses/DataScienceFun.jpg", 
                        Price = 74.99m,
                        Level = "Beginner",
                        Category = "Data Science",
                        DurationInWeeks = 10,
                        LessonsCount = 72,
                        StudentsEnrolled = 2543,
                        Rating = 4.8m,
                        RatingCount = 567,
                        ExternalUrl = "https://www.example.com/data-science-course",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now.AddMonths(-4)
                    },
                    new CourseModel
                    {
                        Title = "Machine Learning Engineering",
                        Description = "Master the techniques and tools used to build production-ready machine learning systems. This course covers supervised and unsupervised learning, neural networks, model deployment, MLOps, performance optimization, and ethical considerations. You'll implement machine learning models to solve real-world problems.",
                        ImageUrl = "/images/courses/ML.jpg",
                        Price = 89.99m,
                        Level = "Advanced",
                        Category = "Data Science",
                        DurationInWeeks = 12,
                        LessonsCount = 86,
                        StudentsEnrolled = 1243,
                        Rating = 4.9m,
                        RatingCount = 312,
                        ExternalUrl = "https://www.example.com/machine-learning-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-5)
                    },

                    // Personal Development Courses
                    new CourseModel
                    {
                        Title = "Effective Communication Skills",
                        Description = "Improve your personal and professional communication abilities. This course covers verbal and non-verbal communication, active listening, presentation skills, conflict resolution, negotiation, and persuasion techniques. You'll become more confident and effective in all your interactions.",
                        ImageUrl = "/images/courses/Eff.jpg", 
                        Price = 39.99m,
                        Level = "All Levels",
                        Category = "Personal Development",
                        DurationInWeeks = 4,
                        LessonsCount = 28,
                        StudentsEnrolled = 3245,
                        Rating = 4.6m,
                        RatingCount = 876,
                        ExternalUrl = "https://www.example.com/communication-course",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now.AddMonths(-7)
                    },
                    new CourseModel
                    {
                        Title = "Leadership and Management Skills",
                        Description = "Develop essential leadership and management capabilities to inspire teams and drive results. This course covers leadership styles, team building, motivation, delegation, performance management, strategic thinking, and organizational development. You'll gain practical skills to lead effectively in any organization.",
                        ImageUrl = "/images/courses/LeaderShip.jpg", 
                        Price = 59.99m,
                        Level = "Intermediate",
                        Category = "Personal Development",
                        DurationInWeeks = 6,
                        LessonsCount = 45,
                        StudentsEnrolled = 1876,
                        Rating = 4.7m,
                        RatingCount = 432,
                        ExternalUrl = "https://www.example.com/leadership-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-8)
                    },
                    new CourseModel
                    {
                        Title = "Productivity and Time Management",
                        Description = "Master techniques to maximize your productivity and effectively manage your time. This course covers goal setting, prioritization, planning systems, focus techniques, habit formation, and overcoming procrastination. You'll learn how to accomplish more with less stress and achieve better work-life balance.",
                        ImageUrl = "/images/courses/Time.jpg", 
                        Price = 44.99m,
                        Level = "All Levels",
                        Category = "Personal Development",
                        DurationInWeeks = 4,
                        LessonsCount = 32,
                        StudentsEnrolled = 2543,
                        Rating = 4.5m,
                        RatingCount = 587,
                        ExternalUrl = "https://www.example.com/productivity-course",
                        IsFeatured = false,
                        CreatedAt = DateTime.Now.AddMonths(-9)
                    }
                );

                context.SaveChanges(); // save in database // add maulaly 
            }
        
    }
    }
}