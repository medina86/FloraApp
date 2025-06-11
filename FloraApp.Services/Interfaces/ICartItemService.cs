using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;

namespace FloraApp.Services.Interfaces
{
    public interface ICartItemService : ICrudService<
        CartItemResponse,
        CartItemSearchObject,
        CartItemInsertRequest,
        CartItemUpdateRequest>
    {
        Task<CartItemResponse?> GetCartItemByProductAsync(int cartId, int productId);
        Task<bool> ItemExistsInCartAsync(int cartId, int productId);
        Task<List<CartItemResponse>> GetCartItemsAsync(int cartId);
    }
} 