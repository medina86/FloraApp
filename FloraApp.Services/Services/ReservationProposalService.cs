using MapsterMapper;
using FloraApp.Services.Database;
using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using FloraApp.Services.StateMachine;
using FloraApp.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace FloraApp.Services.Services
{
    public class ReservationProposalService : BaseCrudService<
        ReservationProposalResponse,
        ReservationProposalSearchObject,
        ReservationProposal,
        ReservationProposalInsertRequest,
        ReservationProposalUpdateRequest>,
        IReservationProposalService
    {
        private readonly ProposalStateMachine _stateMachine;

        public ReservationProposalService(FloraAppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _stateMachine = new ProposalStateMachine();
        }

        public async Task<List<ReservationProposalResponse>> GetProposalsForReservationAsync(int reservationId)
        {
            var proposals = await _context.ReservationProposals
                .Include(rp => rp.Reservation)
                .Where(rp => rp.ReservationId == reservationId)
                .OrderByDescending(rp => rp.SentAt)
                .ToListAsync();

            return proposals.Select(MapToResponse).ToList();
        }

        public async Task<ReservationProposalResponse> UpdateProposalStatusAsync(int id, string status)
        {
            var proposal = await _context.ReservationProposals.FindAsync(id);
            if (proposal == null)
                throw new ArgumentException("Proposal not found");

            var newStatus = (ProposalStatus)Enum.Parse(typeof(ProposalStatus), status);
            var success = await _stateMachine.TryTransitionAsync(proposal, newStatus, _context);
            
            if (!success)
            {
                var currentStatus = (ProposalStatus)Enum.Parse(typeof(ProposalStatus), proposal.Status);
                var allowedTransitions = _stateMachine.GetAllowedTransitions(currentStatus);
                throw new InvalidOperationException($"Invalid transition from {currentStatus} to {newStatus}. Allowed transitions: {string.Join(", ", allowedTransitions)}");
            }

            await _context.SaveChangesAsync();
            return MapToResponse(proposal);
        }

        public async Task<List<ReservationProposalResponse>> GetActiveProposalsAsync()
        {
            var proposals = await _context.ReservationProposals
                .Include(rp => rp.Reservation)
                .Where(rp => rp.Status == ProposalStatus.Sent.ToString() && (rp.ExpiresAt == null || rp.ExpiresAt > DateTime.UtcNow))
                .OrderByDescending(rp => rp.SentAt)
                .ToListAsync();

            return proposals.Select(MapToResponse).ToList();
        }

        public async Task<List<ReservationProposalResponse>> GetExpiredProposalsAsync()
        {
            var proposals = await _context.ReservationProposals
                .Include(rp => rp.Reservation)
                .Where(rp => rp.Status == ProposalStatus.Sent.ToString() && rp.ExpiresAt != null && rp.ExpiresAt <= DateTime.UtcNow)
                .OrderByDescending(rp => rp.SentAt)
                .ToListAsync();

            return proposals.Select(MapToResponse).ToList();
        }

        public async Task<List<ProposalStatus>> GetAllowedTransitionsAsync(int proposalId)
        {
            var proposal = await _context.ReservationProposals.FindAsync(proposalId);
            if (proposal == null)
                throw new ArgumentException("Proposal not found");

            var currentStatus = (ProposalStatus)Enum.Parse(typeof(ProposalStatus), proposal.Status);
            return _stateMachine.GetAllowedTransitions(currentStatus);
        }

        public async Task<string> GetStatusDescriptionAsync(int proposalId)
        {
            var proposal = await _context.ReservationProposals.FindAsync(proposalId);
            if (proposal == null)
                throw new ArgumentException("Proposal not found");

            var status = (ProposalStatus)Enum.Parse(typeof(ProposalStatus), proposal.Status);
            return _stateMachine.GetStatusDescription(status);
        }

        public async Task ProcessExpiredProposalsAsync()
        {
            var proposals = await _context.ReservationProposals
                .Where(rp => rp.Status == ProposalStatus.Sent.ToString() && rp.ExpiresAt != null && rp.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();

            foreach (var proposal in proposals)
            {
                await _stateMachine.TryTransitionAsync(proposal, ProposalStatus.Expired, _context);
            }

            await _context.SaveChangesAsync();
        }

        protected override IQueryable<ReservationProposal> AddFilter(IQueryable<ReservationProposal> query, ReservationProposalSearchObject search)
        {
            if (search?.ReservationId.HasValue == true)
                query = query.Where(rp => rp.ReservationId == search.ReservationId.Value);

            if (!string.IsNullOrEmpty(search?.Status))
                query = query.Where(rp => rp.Status == search.Status);

            if (search?.SentAtFrom.HasValue == true)
                query = query.Where(rp => rp.SentAt >= search.SentAtFrom.Value);

            if (search?.SentAtTo.HasValue == true)
                query = query.Where(rp => rp.SentAt <= search.SentAtTo.Value);

            return query;
        }

        protected override ReservationProposalResponse MapToResponse(ReservationProposal entity)
        {
            return _mapper.Map<ReservationProposalResponse>(entity);
        }

        protected override async Task BeforeInsert(ReservationProposal entity, ReservationProposalInsertRequest request)
        {
            entity.SentAt = DateTime.UtcNow;
            entity.Status = ProposalStatus.Sent.ToString();
        }
    }
} 