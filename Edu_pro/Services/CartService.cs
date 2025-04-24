using EduPro.Data;
using EduPro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduPro.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context; 
        private readonly ICourseService _courseService;

        public CartService(ApplicationDbContext context, ICourseService courseService)
        {
            _context = context;
            _courseService = courseService;
        }
        // list of cart items l kol user
        public async Task<List<CartItemModel>> GetCartItemsAsync(int userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .Include(ci => ci.Course)
                .ToListAsync();
        }


        public async Task<CartItemModel> AddToCartAsync(int userId, int courseId)
        {
            // B4of al course da lsa 3ndy fel database wla l2a
            var course = await _courseService.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                throw new ArgumentException($"Course with ID {courseId} not found");
            }

            // B4of lwa hwa enrolleed wla l2a
            var isEnrolled = await _context.UserCourses
                .AnyAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

            if (isEnrolled)
            {
                throw new InvalidOperationException("You are already enrolled in this course");
            }

            // b4of lw hwa f3ln 3amel add to cart 
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.CourseId == courseId);

            if (existingItem != null)
            {
                return existingItem;
            }

             // lw kol aly fat 8alt 
            var cartItem = new CartItemModel
            {
                UserId = userId,
                CourseId = courseId,
                AddedAt = DateTime.UtcNow
            };

            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            return cartItem;
        }
        // remove button 
        public async Task<bool> RemoveFromCartAsync(int userId, int cartItemId)
        {

            // bgeab al cart bt3at al user da 
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

            if (cartItem == null)
            {
                return false;
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> ClearCartAsync(int userId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return false;
            }

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<int> GetCartItemCountAsync(int userId)
        {
            return await _context.CartItems
                .CountAsync(ci => ci.UserId == userId);
        }
        // check if is already in cart 
        public async Task<bool> IsInCartAsync(int userId, int courseId)
        {
            return await _context.CartItems
                .AnyAsync(ci => ci.UserId == userId && ci.CourseId == courseId);
        }
    }
}