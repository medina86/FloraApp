using System.ComponentModel.DataAnnotations;

namespace FloraApp.Model.Requests
{
    public class CustomBouquetUpsertRequest
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        
        public string? Note { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;
    }
} 