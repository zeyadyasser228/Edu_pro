// Services/ICartService.cs
using EduPro.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduPro.Services
{
    public interface ICartService
    {
        Task<List<CartItemModel>> GetCartItemsAsync(int userId);
        Task<CartItemModel> AddToCartAsync(int userId, int courseId);
        Task<bool> RemoveFromCartAsync(int userId, int cartItemId);
        Task<bool> ClearCartAsync(int userId);
        Task<int> GetCartItemCountAsync(int userId);
        Task<bool> IsInCartAsync(int userId, int courseId);
    }
}