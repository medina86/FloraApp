using System;

namespace FloraApp.Model.Requests
{
    public class OrderInsertRequest
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
    }
} 