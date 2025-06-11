namespace FloraApp.Model.Requests
{
    public class ReservationProposalUpdateRequest
    {
        public string ProposalText { get; set; }
        public decimal PriceEstimate { get; set; }
        public string Status { get; set; }
        public string? AdminNotes { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
} 