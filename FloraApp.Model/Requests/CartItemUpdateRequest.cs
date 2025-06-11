namespace FloraApp.Model.Requests
{
    public class CartItemUpdateRequest
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
} 