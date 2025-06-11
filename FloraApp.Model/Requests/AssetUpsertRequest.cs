using System.ComponentModel.DataAnnotations;

namespace FloraApp.Model.Requests
{
    public class AssetUpsertRequest
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public string Base64Content { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string ContentType { get; set; } = string.Empty; // e.g., "image/jpeg", "image/png"
        
        [MaxLength(255)]
        public string? Title { get; set; }
        
        public bool IsPrimary { get; set; } = false;
    }
} 