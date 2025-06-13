using System;

namespace FloraApp.Model.SearchObjects
{
    public class BlogPostSearchObject : BaseSearchObject
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
} 