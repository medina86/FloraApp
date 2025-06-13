using System.ComponentModel.DataAnnotations;

namespace FloraApp.Model.Requests
{
    public class BlogPostUpsertRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; }
    }
} 