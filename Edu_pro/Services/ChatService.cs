using OpenAI_API;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace EduPro.Services
{
    public class ChatService : IChatService
    {
        private readonly OpenAIAPI _openAI;
        private readonly IConfiguration _configuration;
        private DateTime _lastRequestTime = DateTime.MinValue;
        private const int MIN_REQUEST_INTERVAL_MS = 1000; // Minimum 1 second between requests

        public ChatService(IConfiguration configuration)
        {
            _configuration = configuration;
            var apiKey = configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OpenAI API key is not configured.");
            }
            _openAI = new OpenAIAPI(apiKey);
        }

        public async Task<string> GetChatResponseAsync(string userMessage)
        {
            try
            {
                // Rate limiting
                var timeSinceLastRequest = DateTime.Now - _lastRequestTime;
                if (timeSinceLastRequest.TotalMilliseconds < MIN_REQUEST_INTERVAL_MS)
                {
                    await Task.Delay(MIN_REQUEST_INTERVAL_MS - (int)timeSinceLastRequest.TotalMilliseconds);
                }

                var chat = _openAI.Chat.CreateConversation();
                chat.AppendSystemMessage("You are a helpful educational assistant. Keep responses concise and focused on helping students learn.");
                chat.AppendUserInput(userMessage);
                
                string response = await chat.GetResponseFromChatbotAsync();
                _lastRequestTime = DateTime.Now;
                return response;
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("TooManyRequests"))
            {
                return "I apologize, but we've reached our usage limit. Please try again in a few moments.";
            }
            catch (Exception ex)
            {
                // Log the error details here if you have logging configured
                if (ex.Message.Contains("api.openai.com"))
                {
                    return "There seems to be an issue connecting to the AI service. Please check your internet connection and try again.";
                }
                return "I apologize, but I'm having trouble responding right now. Please try again in a few moments.";
            }
        }
    }
} 