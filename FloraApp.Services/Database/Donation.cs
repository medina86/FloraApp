using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraApp.Services.Database
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCollected { get; set; }
    }
} 