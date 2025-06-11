using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraApp.Services.Database
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public string Base64Content { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string ContentType { get; set; } = string.Empty; // e.g., "image/jpeg", "image/png"
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [MaxLength(255)]
        public string? Title { get; set; }
        
        public bool IsPrimary { get; set; } = false;
        
        // Navigation property
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
} 