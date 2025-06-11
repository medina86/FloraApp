namespace FloraApp.Model.Requests
{
    public class ReservationProposalInsertRequest
    {
        public int ReservationId { get; set; }
        public string ProposalText { get; set; }
        public decimal PriceEstimate { get; set; }
        public string? AdminNotes { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
} 