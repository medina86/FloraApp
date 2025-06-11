using System;

namespace FloraApp.Model.Responses
{
    public class CustomBouquetItemResponse
    {
        public int Id { get; set; }
        public int CustomBouquetId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Total price for this item (ProductPrice * Quantity)
    }
} 