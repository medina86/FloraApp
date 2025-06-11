using FloraApp.Model.SearchObjects;

namespace FloraApp.Model.SearchObjects
{
    public class ReservationProposalSearchObject : BaseSearchObject
    {
        public int? ReservationId { get; set; }
        public string? Status { get; set; }
        public DateTime? SentAtFrom { get; set; }
        public DateTime? SentAtTo { get; set; }
    }
} 