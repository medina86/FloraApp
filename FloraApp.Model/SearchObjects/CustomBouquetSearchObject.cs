using System;

namespace FloraApp.Model.SearchObjects
{
    public class CustomBouquetSearchObject : BaseSearchObject
    {
        public int? UserId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
} 