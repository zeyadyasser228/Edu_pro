using System;
using System.Collections.Generic;
using System.Linq;

namespace EduPro.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>(); // 2 
        public decimal TotalPrice => Items.Sum(i => i.Price); // sum is linq 
        public int ItemCount => Items.Count; // 2
    }

    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Level { get; set; }
        public int DurationInWeeks { get; set; }
        public int LessonsCount { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}