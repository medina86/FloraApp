using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Model.Enums;

namespace FloraApp.Services.Interfaces
{
    public interface IReservationService : ICrudService<
        ReservationResponse,
        ReservationSearchObject,
        ReservationInsertRequest,
        ReservationUpdateRequest>
    {
        Task<List<ReservationResponse>> GetUserReservationsAsync(int userId);
        Task<ReservationResponse> UpdateReservationStatusAsync(int id, string status, string? adminNotes = null);
        Task<List<ReservationResponse>> GetPendingReservationsAsync();
        Task<List<ReservationResponse>> GetReservationsByDateRangeAsync(DateTime from, DateTime to);
        Task<ReservationResponse> GetReservationWithProposalsAsync(int id);
        Task<List<ReservationStatus>> GetAllowedTransitionsAsync(int reservationId);
        Task<string> GetStatusDescriptionAsync(int reservationId);
    }
} 