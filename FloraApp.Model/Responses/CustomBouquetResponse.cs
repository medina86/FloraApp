using System;
using System.Collections.Generic;

namespace FloraApp.Model.Responses
{
    public class CustomBouquetResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<CustomBouquetItemResponse> Items { get; set; } = new List<CustomBouquetItemResponse>();
    }
} 