using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloraApp.Services.Interfaces
{
    public interface ICustomBouquetItemService
    {
        Task<List<CustomBouquetItemResponse>> GetByBouquetIdAsync(int bouquetId);
        Task<CustomBouquetItemResponse> AddItemAsync(int bouquetId, CustomBouquetItemUpsertRequest request);
        Task<CustomBouquetItemResponse> UpdateItemAsync(int itemId, CustomBouquetItemUpsertRequest request);
        Task DeleteItemAsync(int itemId);
        Task UpdateBouquetPriceAsync(int bouquetId);
    }
} 