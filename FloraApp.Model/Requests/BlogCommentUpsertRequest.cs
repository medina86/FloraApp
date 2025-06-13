using System.ComponentModel.DataAnnotations;

namespace FloraApp.Model.Requests
{
    public class BlogCommentUpsertRequest
    {
        [Required]
        public int BlogPostId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;
    }
} 