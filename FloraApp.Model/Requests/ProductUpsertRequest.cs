using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FloraApp.Model.Requests
{
    public class ProductUpsertRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        [Range(0.01, 9999.99)]
        public decimal Price { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public int? CategoryId { get; set; }
        
        public List<int>? AssetIds { get; set; }
    }
} 