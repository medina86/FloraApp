using System.ComponentModel.DataAnnotations;

namespace FloraApp.Model.Requests
{
    public class CustomBouquetItemUpsertRequest
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
} 