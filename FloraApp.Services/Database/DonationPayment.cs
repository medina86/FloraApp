using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraApp.Services.Database
{
    public class DonationPayment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int DonationId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        public string? Purpose { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        // Navigation properties
        [ForeignKey("DonationId")]
        public virtual Donation Donation { get; set; } = null!;
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
} 