using FloraApp.Model.SearchObjects;

namespace FloraApp.Model.SearchObjects
{
    public class CartItemSearchObject : BaseSearchObject
    {
        public int? CartId { get; set; }
        public int? ProductId { get; set; }
    }
} 