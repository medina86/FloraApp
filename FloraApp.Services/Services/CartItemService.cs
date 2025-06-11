using MapsterMapper;
using FloraApp.Services.Database;
using FloraApp.Services.Services;
using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FloraApp.Services.Services
{
    public class CartItemService : BaseCrudService<
        CartItemResponse,
        CartItemSearchObject,
        CartItem,
        CartItemInsertRequest,
        CartItemUpdateRequest>,
        ICartItemService
    {
        public CartItemService(FloraAppDbContext context, IMapper mapper) : base(context, mapper)
        {
            
        }

        public async Task<CartItemResponse?> GetCartItemByProductAsync(int cartId, int productId)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Assets)
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (cartItem == null)
                return null;

            return new CartItemResponse
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product?.Name ?? "",
                ProductImage = cartItem.Product?.Assets?.FirstOrDefault()?.Base64Content ?? "",
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
                TotalPrice = cartItem.Price * cartItem.Quantity
            };
        }

        public async Task<bool> ItemExistsInCartAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .AnyAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task<List<CartItemResponse>> GetCartItemsAsync(int cartId)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Assets)
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            return cartItems.Select(ci => new CartItemResponse
            {
                Id = ci.Id,
                CartId = ci.CartId,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name ?? "",
                ProductImage = ci.Product?.Assets?.FirstOrDefault()?.Base64Content ?? "",
                Quantity = ci.Quantity,
                Price = ci.Price,
                TotalPrice = ci.Price * ci.Quantity
            }).ToList();
        }

        protected override CartItemResponse MapToResponse(CartItem entity)
        {
            return new CartItemResponse
            {
                Id = entity.Id,
                CartId = entity.CartId,
                ProductId = entity.ProductId,
                ProductName = "", // Ovo će biti popunjeno kada se uključi Product
                ProductImage = "",
                Quantity = entity.Quantity,
                Price = entity.Price,
                TotalPrice = entity.Price * entity.Quantity
            };
        }
    }
} 