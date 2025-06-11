using System;
using System.ComponentModel.DataAnnotations;

namespace FloraApp.Services.Database
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
    }
} 