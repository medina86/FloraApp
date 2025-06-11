namespace FloraApp.Model.Responses
{
    public class ReservationProposalResponse
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string ProposalText { get; set; }
        public decimal PriceEstimate { get; set; }
        public DateTime SentAt { get; set; }
        public string Status { get; set; }
        public string? AdminNotes { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
} 