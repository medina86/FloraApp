using System;

namespace FloraApp.Model.SearchObjects
{
    public class BlogCommentSearchObject : BaseSearchObject
    {
        public int? BlogPostId { get; set; }
        public int? UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
} 