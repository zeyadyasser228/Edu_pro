using Azure;
using Azure.AI.Language.Conversations;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace EduPro.Services
{
    public class AzureChatService : IChatService
    {
        private readonly ConversationAnalysisClient _client;
        private readonly string _projectName;
        private readonly string _deploymentName;

        public AzureChatService(IConfiguration configuration)
        {
            var endpoint = configuration["Azure:LanguageService:Endpoint"];
            var key = configuration["Azure:LanguageService:Key"];
            _projectName = configuration["Azure:LanguageService:ProjectName"];
            _deploymentName = configuration["Azure:LanguageService:DeploymentName"];

            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("Azure Language Service configuration is missing.");
            }

            _client = new ConversationAnalysisClient(
                new Uri(endpoint),
                new AzureKeyCredential(key));
        }

        public async Task<string> GetChatResponseAsync(string userMessage)
        {
            try
            {
                var data = new
                {
                    analysisInput = new
                    {
                        conversationItem = new
                        {
                            text = userMessage,
                            id = "1",
                            participantId = "user1"
                        }
                    },
                    parameters = new
                    {
                        projectName = _projectName,
                        deploymentName = _deploymentName,
                        verbose = true
                    },
                    kind = "Conversation"
                };

                Response response = await _client.AnalyzeConversationAsync(RequestContent.Create(data));
                var result = response.Content.ToString();

                // For now, return a simplified response
                return $"I understand you're asking about: {userMessage}. How can I help you learn more about this topic?";
            }
            catch
            {
                // Log the error
                return "I apologize, but I'm having trouble understanding. Could you rephrase your question?";
            }
        }
    }
} 