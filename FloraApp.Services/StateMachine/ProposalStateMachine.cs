using FloraApp.Model.Enums;
using FloraApp.Services.Database;

namespace FloraApp.Services.StateMachine
{
    public class ProposalStateMachine
    {
        private readonly Dictionary<ProposalStatus, List<ProposalStatus>> _allowedTransitions;

        public ProposalStateMachine()
        {
            _allowedTransitions = new Dictionary<ProposalStatus, List<ProposalStatus>>
            {
                [ProposalStatus.Draft] = new List<ProposalStatus> 
                { 
                    ProposalStatus.Sent, 
                    ProposalStatus.Withdrawn 
                },
                
                [ProposalStatus.Sent] = new List<ProposalStatus> 
                { 
                    ProposalStatus.Accepted, 
                    ProposalStatus.Rejected, 
                    ProposalStatus.Expired, 
                    ProposalStatus.Withdrawn 
                },
                
                [ProposalStatus.Accepted] = new List<ProposalStatus>(), // Terminal state
                
                [ProposalStatus.Rejected] = new List<ProposalStatus>(), // Terminal state
                
                [ProposalStatus.Expired] = new List<ProposalStatus>(), // Terminal state
                
                [ProposalStatus.Withdrawn] = new List<ProposalStatus>() // Terminal state
            };
        }

        public bool CanTransition(ProposalStatus currentStatus, ProposalStatus newStatus)
        {
            return _allowedTransitions.ContainsKey(currentStatus) && 
                   _allowedTransitions[currentStatus].Contains(newStatus);
        }

        public List<ProposalStatus> GetAllowedTransitions(ProposalStatus currentStatus)
        {
            return _allowedTransitions.ContainsKey(currentStatus) 
                ? _allowedTransitions[currentStatus] 
                : new List<ProposalStatus>();
        }

        public async Task<bool> TryTransitionAsync(ReservationProposal proposal, ProposalStatus newStatus, FloraAppDbContext context)
        {
            var currentStatus = (ProposalStatus)Enum.Parse(typeof(ProposalStatus), proposal.Status);
            
            if (!CanTransition(currentStatus, newStatus))
            {
                return false;
            }

            // Izvrši dodatne akcije pri tranziciji
            await ExecuteTransitionActionsAsync(proposal, currentStatus, newStatus, context);
            
            proposal.Status = newStatus.ToString();
            return true;
        }

        private async Task ExecuteTransitionActionsAsync(ReservationProposal proposal, ProposalStatus fromStatus, ProposalStatus toStatus, FloraAppDbContext context)
        {
            switch (toStatus)
            {
                case ProposalStatus.Sent:
                    proposal.SentAt = DateTime.UtcNow;
                    break;
                    
                case ProposalStatus.Accepted:
                    // Ažuriraj status rezervacije na Accepted
                    var reservation = await context.Reservations.FindAsync(proposal.ReservationId);
                    if (reservation != null)
                    {
                        reservation.Status = ReservationStatus.Accepted.ToString();
                    }
                    break;
                    
                case ProposalStatus.Rejected:
                    // Možda ažuriraj status rezervacije na Rejected
                    var reservationRejected = await context.Reservations.FindAsync(proposal.ReservationId);
                    if (reservationRejected != null)
                    {
                        reservationRejected.Status = ReservationStatus.Rejected.ToString();
                    }
                    break;
                    
                case ProposalStatus.Expired:
                    // Automatski se postavlja kada istekne rok
                    break;
                    
                case ProposalStatus.Withdrawn:
                    // Administrator povukao prijedlog
                    break;
            }
        }

        public string GetStatusDescription(ProposalStatus status)
        {
            return status switch
            {
                ProposalStatus.Draft => "Prijedlog u pripremi",
                ProposalStatus.Sent => "Prijedlog poslan korisniku",
                ProposalStatus.Accepted => "Prijedlog prihvaćen",
                ProposalStatus.Rejected => "Prijedlog odbijen",
                ProposalStatus.Expired => "Prijedlog istekao",
                ProposalStatus.Withdrawn => "Prijedlog povučen",
                _ => "Nepoznat status"
            };
        }

        public bool ShouldAutoExpire(ReservationProposal proposal)
        {
            if (proposal.Status != ProposalStatus.Sent.ToString() || proposal.ExpiresAt == null)
                return false;

            return proposal.ExpiresAt <= DateTime.UtcNow;
        }
    }
} 