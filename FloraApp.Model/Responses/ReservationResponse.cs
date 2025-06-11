using System.Collections.Generic;

namespace FloraApp.Model.Responses
{
    public class ReservationResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string EventType { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; }
        public int? GuestNumber { get; set; }
        public int? TableCount { get; set; }
        public string? Location { get; set; }
        public string? Style { get; set; }
        public string? ColorTheme { get; set; }
        public decimal? Budget { get; set; }
        public string? SpecialRequests { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string? AdminNotes { get; set; }
        public List<ReservationProposalResponse> Proposals { get; set; } = new List<ReservationProposalResponse>();
    }
} 