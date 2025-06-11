using System.Collections.Generic;

namespace FloraApp.Model.Responses
{
    public class CartResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public int ItemCount { get; set; }
        public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
    }
} 