using System;
using System.Collections.Generic;

namespace FloraApp.Model.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<AssetResponse>? Assets { get; set; }
    }
    
    public class AssetResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? Title { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsPrimary { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
} 