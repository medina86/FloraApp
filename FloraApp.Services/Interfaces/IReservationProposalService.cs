using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Model.Enums;

namespace FloraApp.Services.Interfaces
{
    public interface IReservationProposalService : ICrudService<
        ReservationProposalResponse,
        ReservationProposalSearchObject,
        ReservationProposalInsertRequest,
        ReservationProposalUpdateRequest>
    {
        Task<List<ReservationProposalResponse>> GetProposalsForReservationAsync(int reservationId);
        Task<ReservationProposalResponse> UpdateProposalStatusAsync(int id, string status);
        Task<List<ReservationProposalResponse>> GetActiveProposalsAsync();
        Task<List<ReservationProposalResponse>> GetExpiredProposalsAsync();
        Task<List<ProposalStatus>> GetAllowedTransitionsAsync(int proposalId);
        Task<string> GetStatusDescriptionAsync(int proposalId);
        Task ProcessExpiredProposalsAsync();
    }
} 