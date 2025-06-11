using FloraApp.Model.SearchObjects;

namespace FloraApp.Model.SearchObjects
{
    public class OrderItemSearchObject : BaseSearchObject
    {
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
    }
} 