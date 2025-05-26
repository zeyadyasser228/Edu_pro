using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EduPro.Services
{
    public class SimpleChatService : IChatService
    {
        private readonly Dictionary<string, Func<string, string>> _questionHandlers = new Dictionary<string, Func<string, string>>
        {
            ["what"] = (msg) => HandleWhatQuestion(msg),
            ["how"] = (msg) => HandleHowQuestion(msg),
            ["why"] = (msg) => HandleWhyQuestion(msg),
            ["can"] = (msg) => HandleCanQuestion(msg),
            ["where"] = (msg) => HandleWhereQuestion(msg)
        };

        private readonly Dictionary<string, string[]> _topicInfo = new Dictionary<string, string[]>
        {
            ["programming"] = new[]
            {
                "Programming is a way to create software and applications. We offer courses in:",
                "• Web Development (HTML, CSS, JavaScript)",
                "• Mobile App Development (Android, iOS)",
                "• Backend Development (C#, Python, Java)",
                "• Database Management (SQL, MongoDB)",
                "\nWhich area interests you most?"
            },
            ["web"] = new[]
            {
                "Our web development curriculum includes:",
                "• Frontend basics (HTML5, CSS3)",
                "• JavaScript and modern frameworks",
                "• Responsive design principles",
                "• Web security fundamentals",
                "\nWould you like to start with frontend or backend?"
            },
            ["course"] = new[]
            {
                "Our available courses include:",
                "• Programming Fundamentals ($299)",
                "• Web Development Bootcamp ($499)",
                "• Data Science Essentials ($399)",
                "• Mobile App Development ($449)",
                "\nWhich course would you like to know more about?"
            },
            ["price"] = new[]
            {
                "Our course pricing structure:",
                "• Basic courses: $199-$299",
                "• Intermediate courses: $299-$499",
                "• Advanced courses: $499-$699",
                "• We offer student discounts and payment plans",
                "\nWould you like specific course pricing?"
            }
        };

        public async Task<string> GetChatResponseAsync(string userMessage)
        {
            try
            {
                string cleanMessage = userMessage.Trim().ToLower();
                
                // Handle greetings
                if (IsGreeting(cleanMessage))
                {
                    return "Hello! 👋 I'm your educational assistant. I can help you with:\n" +
                           "• Course information and pricing\n" +
                           "• Programming and web development\n" +
                           "• Learning resources and materials\n" +
                           "What would you like to know about?";
                }

                // Handle question types
                string questionType = GetQuestionType(cleanMessage);
                if (_questionHandlers.ContainsKey(questionType))
                {
                    return _questionHandlers[questionType](cleanMessage);
                }

                // Handle specific topics
                foreach (var topic in _topicInfo.Keys)
                {
                    if (cleanMessage.Contains(topic))
                    {
                        return string.Join("\n", _topicInfo[topic]);
                    }
                }

                // Default response with topic detection
                return "I see you're interested in learning. Could you specify what you'd like to know about?\n" +
                       "You can ask about:\n" +
                       "• Our courses and pricing\n" +
                       "• Programming and development\n" +
                       "• Specific technologies or subjects";
            }
            catch
            {
                return "I'm here to help! Could you rephrase your question? You can ask about our courses, pricing, or specific subjects.";
            }
        }

        private bool IsGreeting(string message)
        {
            string[] greetings = { "hi", "hello", "hey", "greetings", "good morning", "good afternoon", "good evening" };
            return greetings.Any(g => message.Contains(g));
        }

        private string GetQuestionType(string message)
        {
            return _questionHandlers.Keys.FirstOrDefault(qt => message.StartsWith(qt)) ?? "";
        }

        private static string HandleWhatQuestion(string message)
        {
            if (message.Contains("course"))
                return "We offer various courses including:\n" +
                       "• Programming Fundamentals\n" +
                       "• Web Development\n" +
                       "• Mobile App Development\n" +
                       "• Data Science\n" +
                       "\nWhich course interests you?";

            if (message.Contains("price") || message.Contains("cost"))
                return "Our courses are priced competitively:\n" +
                       "• Basic courses start at $199\n" +
                       "• Premium courses range from $299-$699\n" +
                       "• Custom learning paths available\n" +
                       "\nWould you like specific course pricing?";

            if (message.Contains("learn"))
                return "You can learn through:\n" +
                       "• Structured online courses\n" +
                       "• Interactive coding exercises\n" +
                       "• Project-based learning\n" +
                       "• One-on-one mentoring\n" +
                       "\nWhat learning style interests you?";

            return "We offer comprehensive education in:\n" +
                   "• Programming & Development\n" +
                   "• Data Science & Analytics\n" +
                   "• Web & Mobile Development\n" +
                   "\nWhat specific area interests you?";
        }

        private static string HandleHowQuestion(string message)
        {
            if (message.Contains("start"))
                return "To get started:\n" +
                       "1. Choose your learning path\n" +
                       "2. Enroll in a course\n" +
                       "3. Access learning materials\n" +
                       "4. Join study groups\n" +
                       "\nWould you like help choosing a path?";

            if (message.Contains("pay") || message.Contains("enroll"))
                return "You can enroll easily:\n" +
                       "1. Select your course\n" +
                       "2. Choose payment plan\n" +
                       "3. Complete registration\n" +
                       "\nShall I show you the course catalog?";

            return "We support your learning through:\n" +
                   "• Structured courses\n" +
                   "• Practical exercises\n" +
                   "• Expert guidance\n" +
                   "• Community support\n" +
                   "\nWhat would you like to know more about?";
        }

        private static string HandleWhyQuestion(string message)
        {
            return "Learning with us offers:\n" +
                   "• Industry-relevant skills\n" +
                   "• Practical experience\n" +
                   "• Career opportunities\n" +
                   "• Professional certification\n" +
                   "\nWould you like to know more about any of these benefits?";
        }

        private static string HandleCanQuestion(string message)
        {
            if (message.Contains("learn"))
                return "Yes! You can learn:\n" +
                       "• At your own pace\n" +
                       "• With flexible schedules\n" +
                       "• Through practical projects\n" +
                       "• With mentor support\n" +
                       "\nWhat would you like to learn first?";

            if (message.Contains("help"))
                return "I can help you with:\n" +
                       "• Course selection\n" +
                       "• Learning materials\n" +
                       "• Technical questions\n" +
                       "• Career guidance\n" +
                       "\nWhat kind of help do you need?";

            return "We can assist you with:\n" +
                   "• Choosing courses\n" +
                   "• Learning paths\n" +
                   "• Technical support\n" +
                   "• Career guidance\n" +
                   "\nWhat would you like assistance with?";
        }

        private static string HandleWhereQuestion(string message)
        {
            return "All our courses are available:\n" +
                   "• Online through our platform\n" +
                   "• Accessible 24/7\n" +
                   "• From any device\n" +
                   "• With cloud-based tools\n" +
                   "\nWould you like to see a demo?";
        }
    }
} 