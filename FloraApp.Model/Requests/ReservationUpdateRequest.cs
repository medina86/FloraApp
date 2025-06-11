namespace FloraApp.Model.Requests
{
    public class ReservationUpdateRequest
    {
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
        public string Status { get; set; }
        public string? AdminNotes { get; set; }
    }
} 