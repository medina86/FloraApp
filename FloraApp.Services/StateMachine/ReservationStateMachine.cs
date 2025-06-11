using FloraApp.Model.Enums;
using FloraApp.Services.Database;

namespace FloraApp.Services.StateMachine
{
    public class ReservationStateMachine
    {
        private readonly Dictionary<ReservationStatus, List<ReservationStatus>> _allowedTransitions;

        public ReservationStateMachine()
        {
            _allowedTransitions = new Dictionary<ReservationStatus, List<ReservationStatus>>
            {
                [ReservationStatus.Pending] = new List<ReservationStatus> 
                { 
                    ReservationStatus.UnderReview, 
                    ReservationStatus.Cancelled 
                },
                
                [ReservationStatus.UnderReview] = new List<ReservationStatus> 
                { 
                    ReservationStatus.ProposalSent, 
                    ReservationStatus.Cancelled 
                },
                
                [ReservationStatus.ProposalSent] = new List<ReservationStatus> 
                { 
                    ReservationStatus.Accepted, 
                    ReservationStatus.Rejected, 
                    ReservationStatus.UnderReview, // Za slanje novog prijedloga
                    ReservationStatus.Cancelled 
                },
                
                [ReservationStatus.Accepted] = new List<ReservationStatus> 
                { 
                    ReservationStatus.InProgress, 
                    ReservationStatus.Cancelled 
                },
                
                [ReservationStatus.Rejected] = new List<ReservationStatus> 
                { 
                    ReservationStatus.UnderReview, // Za slanje novog prijedloga
                    ReservationStatus.Cancelled 
                },
                
                [ReservationStatus.InProgress] = new List<ReservationStatus> 
                { 
                    ReservationStatus.Completed, 
                    ReservationStatus.Cancelled 
                },
                
                [ReservationStatus.Completed] = new List<ReservationStatus>(), // Terminal state
                
                [ReservationStatus.Cancelled] = new List<ReservationStatus>() // Terminal state
            };
        }

        public bool CanTransition(ReservationStatus currentStatus, ReservationStatus newStatus)
        {
            return _allowedTransitions.ContainsKey(currentStatus) && 
                   _allowedTransitions[currentStatus].Contains(newStatus);
        }

        public List<ReservationStatus> GetAllowedTransitions(ReservationStatus currentStatus)
        {
            return _allowedTransitions.ContainsKey(currentStatus) 
                ? _allowedTransitions[currentStatus] 
                : new List<ReservationStatus>();
        }

        public async Task<bool> TryTransitionAsync(Reservation reservation, ReservationStatus newStatus, FloraAppDbContext context)
        {
            var currentStatus = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), reservation.Status);
            
            if (!CanTransition(currentStatus, newStatus))
            {
                return false;
            }

            // Izvrši dodatne akcije pri tranziciji
            await ExecuteTransitionActionsAsync(reservation, currentStatus, newStatus, context);
            
            reservation.Status = newStatus.ToString();
            return true;
        }

        private async Task ExecuteTransitionActionsAsync(Reservation reservation, ReservationStatus fromStatus, ReservationStatus toStatus, FloraAppDbContext context)
        {
            switch (toStatus)
            {
                case ReservationStatus.UnderReview:
                    // Možda poslati notifikaciju administratoru
                    break;
                    
                case ReservationStatus.ProposalSent:
                    // Automatski postavi datum slanja prijedloga
                    break;
                    
                case ReservationStatus.Accepted:
                    // Možda poslati potvrdu korisniku
                    break;
                    
                case ReservationStatus.InProgress:
                    // Možda poslati notifikaciju o početku radova
                    break;
                    
                case ReservationStatus.Completed:
                    // Možda poslati upitnik za ocjenu
                    break;
                    
                case ReservationStatus.Cancelled:
                    // Možda poslati obavijest o otkazivanju
                    break;
            }
        }

        public string GetStatusDescription(ReservationStatus status)
        {
            return status switch
            {
                ReservationStatus.Pending => "Nova rezervacija - čeka na pregled",
                ReservationStatus.UnderReview => "U pregledu - administrator analizira zahtjeve",
                ReservationStatus.ProposalSent => "Prijedlog poslan - čeka odgovor korisnika",
                ReservationStatus.Accepted => "Prijedlog prihvaćen - priprema dekoracije",
                ReservationStatus.Rejected => "Prijedlog odbijen - moguće slanje novog prijedloga",
                ReservationStatus.InProgress => "Dekoracija u toku",
                ReservationStatus.Completed => "Događaj završen",
                ReservationStatus.Cancelled => "Rezervacija otkazana",
                _ => "Nepoznat status"
            };
        }
    }
} 