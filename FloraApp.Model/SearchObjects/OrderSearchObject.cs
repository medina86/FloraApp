using FloraApp.Model.SearchObjects;

namespace FloraApp.Model.SearchObjects
{
    public class OrderSearchObject : BaseSearchObject
    {
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderStatus { get; set; }
    }
} 