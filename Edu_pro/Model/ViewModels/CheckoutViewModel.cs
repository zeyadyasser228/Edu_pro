
using System.Collections.Generic;
using System.Linq;

namespace EduPro.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();

        public decimal TotalAmount { get; set; }

        public void CalculateTotalAmount()
        {
            TotalAmount = CartItems.Sum(item => item.Course.Price);
        }
    }
}