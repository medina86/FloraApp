namespace FloraApp.Model.SearchObjects
{
    public class CategorySearchObject : BaseSearchObject
    {
        public string? Name { get; set; }
        public string? FTS { get; set; } // Full text search
    }
} 