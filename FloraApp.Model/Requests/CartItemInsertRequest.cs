namespace FloraApp.Model.Requests
{
    public class CartItemInsertRequest
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
} 