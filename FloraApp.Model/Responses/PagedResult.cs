using System.Collections.Generic;

namespace FloraApp.Model.Responses
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int? TotalCount { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? TotalPages => TotalCount.HasValue && PageSize.HasValue && PageSize.Value > 0 
            ? (TotalCount.Value + PageSize.Value - 1) / PageSize.Value 
            : null;
        public bool HasPrevious => Page.HasValue && Page.Value > 0;
        public bool HasNext => Page.HasValue && TotalPages.HasValue && Page.Value < TotalPages.Value - 1;
    }
} 