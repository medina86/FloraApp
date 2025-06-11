using FloraApp.Model.SearchObjects;

namespace FloraApp.Model.Requests
{
    public class OrderItemInsertRequest
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
} 