using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraApp.Services.Database
{
    public class ReservationProposal
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        public string ProposalText { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceEstimate { get; set; }
        
        [Required]
        public DateTime SentAt { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "sent"; // sent, accepted, rejected, expired
        
        public string? AdminNotes { get; set; }
        
        public DateTime? ExpiresAt { get; set; }
        
        // Navigation properties
        [ForeignKey("ReservationId")]
        public virtual Reservation Reservation { get; set; } = null!;
    }
} 