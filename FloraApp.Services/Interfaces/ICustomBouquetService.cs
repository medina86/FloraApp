using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using System.Threading.Tasks;

namespace FloraApp.Services.Interfaces
{
    public interface ICustomBouquetService : ICrudService<CustomBouquetResponse, CustomBouquetSearchObject, CustomBouquetUpsertRequest, CustomBouquetUpsertRequest>
    {
        Task<PagedResult<CustomBouquetResponse>> GetByUserIdAsync(int userId);
    }
} 