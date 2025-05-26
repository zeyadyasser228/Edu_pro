using System.Threading.Tasks;

namespace EduPro.Services
{
    public interface IChatService
    {
        Task<string> GetChatResponseAsync(string userMessage);
    }
} 