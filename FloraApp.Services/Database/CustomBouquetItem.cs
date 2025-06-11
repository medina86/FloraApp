using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraApp.Services.Database
{
    public class CustomBouquetItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int CustomBouquetId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        // Navigation properties
        [ForeignKey("CustomBouquetId")]
        public virtual CustomBouquet CustomBouquet { get; set; } = null!;
        
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
} 