using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EduPro.Services
{
    public class LLMRouterService : IChatService
    {
        private readonly Dictionary<string, string[]> _responses;
        private readonly Random _random;

        public LLMRouterService(IConfiguration configuration)
        {
            _random = new Random();
            _responses = new Dictionary<string, string[]>
            {
                ["greeting"] = new[]
                {
                    "👋 Hello! I'm your educational assistant. How can I help you today?",
                    "Welcome to EduPro! I'm here to help you with your learning journey.",
                    "Hi there! I'm your AI tutor. What would you like to learn about?"
                },
                ["programming"] = new[]
                {
                    "Our programming courses cover:\n• Web Development (HTML, CSS, JavaScript)\n• Backend Development (C#, Python)\n• Mobile App Development\n• Database Management\n\nWhich area interests you?",
                    "We offer comprehensive programming education including:\n• Beginner to Advanced Courses\n• Practical Projects\n• Code Reviews\n• Live Coding Sessions\n\nWould you like more details about any of these?"
                },
                ["courses"] = new[]
                {
                    "Here are our popular courses:\n• Programming Fundamentals ($299)\n• Web Development Bootcamp ($499)\n• Mobile App Development ($449)\n• Database Design ($399)\n\nWould you like details about any specific course?",
                    "Our course offerings include:\n• Beginner Courses (Starting at $199)\n• Intermediate Courses ($299-$499)\n• Advanced Specializations ($499+)\n\nShall I tell you more about any of these?"
                },
                ["default"] = new[]
                {
                    "I can help you with:\n• Course Information\n• Programming Topics\n• Learning Resources\n• Enrollment Process\n\nWhat would you like to know more about?",
                    "Let me assist you with:\n• Finding the Right Course\n• Understanding Programming Concepts\n• Course Pricing and Schedule\n• Learning Materials\n\nWhat interests you?"
                }
            };
        }

        public async Task<string> GetChatResponseAsync(string userMessage)
        {
            try
            {
                var cleanMessage = userMessage.Trim().ToLower();

                // Handle greetings
                if (IsGreeting(cleanMessage))
                {
                    return GetRandomResponse("greeting");
                }

                // Handle programming questions
                if (cleanMessage.Contains("program") || cleanMessage.Contains("coding") || cleanMessage.Contains("development"))
                {
                    return GetRandomResponse("programming");
                }

                // Handle course-related questions
                if (cleanMessage.Contains("course") || cleanMessage.Contains("class") || cleanMessage.Contains("price") || cleanMessage.Contains("cost"))
                {
                    return GetRandomResponse("courses");
                }

                // Default response
                return GetRandomResponse("default");
            }
            catch (Exception)
            {
                return "I'm here to help! Please ask about our courses, programming topics, or learning resources.";
            }
        }

        private string GetRandomResponse(string category)
        {
            var responses = _responses.GetValueOrDefault(category, _responses["default"]);
            return responses[_random.Next(responses.Length)];
        }

        private bool IsGreeting(string message)
        {
            var greetings = new[] { "hi", "hello", "hey", "greetings", "good morning", "good afternoon", "good evening" };
            return greetings.Any(g => message.Contains(g));
        }
    }
} 