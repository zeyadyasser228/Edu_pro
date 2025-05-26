using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EduPro.Services
{
    public class HuggingFaceChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string[] _educationalResponses = new[]
        {
            "That's an interesting question about {0}. In education, it's important to understand the fundamentals first.",
            "I'd be happy to help you learn about {0}. Let's break this topic down step by step.",
            "Learning about {0} is exciting! Would you like to know more specific details?",
            "Great question about {0}! In the educational context, this is a very relevant topic.",
            "When studying {0}, it's helpful to connect it with real-world examples.",
            "I understand you're interested in {0}. This is an important topic in our curriculum.",
            "Let's explore {0} together. What specific aspect would you like to focus on?",
            "Your interest in {0} shows great initiative! Would you like to dive deeper into this subject?",
            "That's a thoughtful question about {0}. Let's approach this from an educational perspective.",
            "Learning about {0} can open up many opportunities. What would you like to know specifically?"
        };

        public HuggingFaceChatService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetChatResponseAsync(string userMessage)
        {
            try
            {
                // Clean and prepare the user message
                string cleanMessage = userMessage.Trim().ToLower();
                
                // Get key topics from the message
                string topic = GetMainTopic(cleanMessage);

                // Select a random educational response format
                Random rand = new Random();
                string responseTemplate = _educationalResponses[rand.Next(_educationalResponses.Length)];

                // Format the response with the topic
                string response = string.Format(responseTemplate, topic);

                // Add a follow-up suggestion based on the topic
                response += GetFollowUpSuggestion(topic);

                return response;
            }
            catch (Exception ex)
            {
                return "I'm here to help you learn! Could you please rephrase your question?";
            }
        }

        private string GetMainTopic(string message)
        {
            // Simple topic extraction - can be enhanced
            string[] commonWords = { "what", "how", "why", "is", "are", "can", "could", "would", "the", "a", "an" };
            var words = message.Split(' ');
            
            foreach (var word in words)
            {
                if (!commonWords.Contains(word) && word.Length > 3)
                {
                    return word;
                }
            }
            
            return "this topic";
        }

        private string GetFollowUpSuggestion(string topic)
        {
            string[] suggestions = new[]
            {
                "\n\nWould you like to see some examples?",
                "\n\nShall we look at some practice exercises?",
                "\n\nWould you like to explore related topics?",
                "\n\nWould you like me to explain this in more detail?",
                "\n\nShall we break this down into smaller parts?"
            };

            Random rand = new Random();
            return suggestions[rand.Next(suggestions.Length)];
        }
    }
} 