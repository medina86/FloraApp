using System.ComponentModel.DataAnnotations;

namespace FloraApp.Model.Requests
{
    public class CategoryUpsertRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string? Description { get; set; }
    }
} 