namespace FloraApp.Model.Enums
{
    public enum ReservationStatus
    {
        Pending = 1,        // Nova rezervacija
        UnderReview = 2,    // Administrator pregledava
        ProposalSent = 3,   // Poslan prijedlog korisniku
        Accepted = 4,       // Korisnik prihvatio prijedlog
        Rejected = 5,       // Korisnik odbio prijedlog
        InProgress = 6,     // Dekoracija u toku
        Completed = 7,      // Događaj završen
        Cancelled = 8       // Rezervacija otkazana
    }
} 