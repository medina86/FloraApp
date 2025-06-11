using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraApp.Services.Database
{
    public class BlogComment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int BlogPostId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("BlogPostId")]
        public virtual BlogPost BlogPost { get; set; } = null!;
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
} 