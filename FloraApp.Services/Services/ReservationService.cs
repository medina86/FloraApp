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
    public class ReservationService : BaseCrudService<
        ReservationResponse,
        ReservationSearchObject,
        Reservation,
        ReservationInsertRequest,
        ReservationUpdateRequest>,
        IReservationService
    {
        private readonly ReservationStateMachine _stateMachine;

        public ReservationService(FloraAppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _stateMachine = new ReservationStateMachine();
        }

        public async Task<List<ReservationResponse>> GetUserReservationsAsync(int userId)
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Proposals)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reservations.Select(MapToResponse).ToList();
        }

        public async Task<ReservationResponse> UpdateReservationStatusAsync(int id, string status, string? adminNotes = null)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                throw new ArgumentException("Reservation not found");

            var newStatus = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), status);
            var success = await _stateMachine.TryTransitionAsync(reservation, newStatus, _context);
            
            if (!success)
            {
                var currentStatus = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), reservation.Status);
                var allowedTransitions = _stateMachine.GetAllowedTransitions(currentStatus);
                throw new InvalidOperationException($"Invalid transition from {currentStatus} to {newStatus}. Allowed transitions: {string.Join(", ", allowedTransitions)}");
            }

            if (adminNotes != null)
                reservation.AdminNotes = adminNotes;

            await _context.SaveChangesAsync();
            return await GetReservationWithProposalsAsync(id);
        }

        public async Task<List<ReservationResponse>> GetPendingReservationsAsync()
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Proposals)
                .Where(r => r.Status == ReservationStatus.Pending.ToString())
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();

            return reservations.Select(MapToResponse).ToList();
        }

        public async Task<List<ReservationResponse>> GetReservationsByDateRangeAsync(DateTime from, DateTime to)
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Proposals)
                .Where(r => r.Date >= from && r.Date <= to)
                .OrderBy(r => r.Date)
                .ToListAsync();

            return reservations.Select(MapToResponse).ToList();
        }

        public async Task<ReservationResponse> GetReservationWithProposalsAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Proposals)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                throw new ArgumentException("Reservation not found");

            return MapToResponse(reservation);
        }

        public async Task<List<ReservationStatus>> GetAllowedTransitionsAsync(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null)
                throw new ArgumentException("Reservation not found");

            var currentStatus = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), reservation.Status);
            return _stateMachine.GetAllowedTransitions(currentStatus);
        }

        public async Task<string> GetStatusDescriptionAsync(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null)
                throw new ArgumentException("Reservation not found");

            var status = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), reservation.Status);
            return _stateMachine.GetStatusDescription(status);
        }

        protected override IQueryable<Reservation> AddFilter(IQueryable<Reservation> query, ReservationSearchObject search)
        {
            if (search?.UserId.HasValue == true)
                query = query.Where(r => r.UserId == search.UserId.Value);

            if (!string.IsNullOrEmpty(search?.EventType))
                query = query.Where(r => r.EventType.Contains(search.EventType));

            if (search?.DateFrom.HasValue == true)
                query = query.Where(r => r.Date >= search.DateFrom.Value);

            if (search?.DateTo.HasValue == true)
                query = query.Where(r => r.Date <= search.DateTo.Value);

            if (!string.IsNullOrEmpty(search?.Status))
                query = query.Where(r => r.Status == search.Status);

            if (search?.BudgetFrom.HasValue == true)
                query = query.Where(r => r.Budget >= search.BudgetFrom.Value);

            if (search?.BudgetTo.HasValue == true)
                query = query.Where(r => r.Budget <= search.BudgetTo.Value);

            return query;
        }

        protected override ReservationResponse MapToResponse(Reservation entity)
        {
            var response = _mapper.Map<ReservationResponse>(entity);
            
            // Map user information
            if (entity.User != null)
            {
                response.UserName = $"{entity.User.FirstName} {entity.User.LastName}";
                response.UserEmail = entity.User.Email;
            }

            // Map proposals
            if (entity.Proposals != null)
            {
                response.Proposals = entity.Proposals.Select(p => new ReservationProposalResponse
                {
                    Id = p.Id,
                    ReservationId = p.ReservationId,
                    ProposalText = p.ProposalText,
                    PriceEstimate = p.PriceEstimate,
                    SentAt = p.SentAt,
                    Status = p.Status,
                    AdminNotes = p.AdminNotes,
                    ExpiresAt = p.ExpiresAt
                }).ToList();
            }

            return response;
        }

        protected override async Task BeforeInsert(Reservation entity, ReservationInsertRequest request)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.Status = ReservationStatus.Pending.ToString();
        }
    }
} 