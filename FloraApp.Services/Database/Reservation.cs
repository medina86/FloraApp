using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraApp.Services.Database
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string EventType { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TimeSlot { get; set; } = string.Empty; // npr. "09:00-12:00", "14:00-18:00"
        
        public int? GuestNumber { get; set; }
        
        public int? TableCount { get; set; }
        
        [MaxLength(100)]
        public string? Location { get; set; }
        
        [MaxLength(50)]
        public string? Style { get; set; }
        
        [MaxLength(100)]
        public string? ColorTheme { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Budget { get; set; }
        
        public string? SpecialRequests { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, reviewed, accepted, rejected, completed
        
        public string? AdminNotes { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        public virtual ICollection<ReservationProposal> Proposals { get; set; } = new List<ReservationProposal>();
    }
} 