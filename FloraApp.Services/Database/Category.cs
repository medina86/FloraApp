using FloraApp.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FloraApp.Services.Database
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
        
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category> Subcategories { get; set; } = new List<Category>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
} 