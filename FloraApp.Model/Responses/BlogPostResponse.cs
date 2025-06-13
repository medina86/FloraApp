using System;

namespace FloraApp.Model.Responses
{
    public class BlogPostResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CommentCount { get; set; }
    }
} 