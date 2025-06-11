using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraApp.Services.Database
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        // Legacy field maintained for backward compatibility
        public string? ImageUrl { get; set; }
        
        // Reference to category
        public int? CategoryId { get; set; }
        
        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        
        public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<CustomBouquetItem> CustomBouquetItems { get; set; } = new List<CustomBouquetItem>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
} 