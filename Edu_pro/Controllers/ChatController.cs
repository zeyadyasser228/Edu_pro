using Microsoft.AspNetCore.Mvc;
using EduPro.Services;
using System.Threading.Tasks;

namespace EduPro.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> GetResponse([FromBody] string message)
        {
            var response = await _chatService.GetChatResponseAsync(message);
            return Json(new { response });
        }
    }
} 