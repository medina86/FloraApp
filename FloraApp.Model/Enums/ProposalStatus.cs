namespace FloraApp.Model.Enums
{
    public enum ProposalStatus
    {
        Draft = 1,          // Prijedlog u pripremi
        Sent = 2,           // Poslan korisniku
        Accepted = 3,       // Korisnik prihvatio
        Rejected = 4,       // Korisnik odbio
        Expired = 5,        // Istekao rok
        Withdrawn = 6       // Administrator povukao
    }
} 