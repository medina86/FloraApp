using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;

namespace FloraApp.Services.Interfaces
{
    public interface ICartService : ICrudService<
        CartResponse,
        CartSearchObject,
        CartInsertRequest,
        CartUpdateRequest>
    {
        Task<CartResponse> GetUserCartAsync(int userId);
        Task<CartResponse> ClearCartAsync(int cartId);
        Task<decimal> CalculateCartTotalAsync(int cartId);
        Task<CartResponse> AddItemToCartAsync(int cartId, CartItemInsertRequest item);
        Task<CartResponse> RemoveItemFromCartAsync(int cartId, int productId);
        Task<CartResponse> UpdateItemQuantityAsync(int cartId, int productId, int quantity);
    }
} 