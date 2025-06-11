using MapsterMapper;
using FloraApp.Services.Database;
using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FloraApp.Services.Services
{
    public class CartService : BaseCrudService<
        CartResponse,
        CartSearchObject,
        Cart,
        CartInsertRequest,
        CartUpdateRequest>,
        ICartService
    {
        private readonly ICartItemService _cartItemService;

        public CartService(FloraAppDbContext context, IMapper mapper, ICartItemService cartItemService) : base(context, mapper)
        {
            _cartItemService = cartItemService;
        }

        public async Task<CartResponse> GetUserCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // Kreiraj novu korpu ako ne postoji
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return await MapCartToResponseAsync(cart);
        }

        public async Task<CartResponse> ClearCartAsync(int cartId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            var cart = await _context.Carts.FindAsync(cartId);
            return await MapCartToResponseAsync(cart);
        }

        public async Task<decimal> CalculateCartTotalAsync(int cartId)
        {
            var total = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .SumAsync(ci => ci.Price * ci.Quantity);

            return total;
        }

        public async Task<CartResponse> AddItemToCartAsync(int cartId, CartItemInsertRequest item)
        {
            // Provjeri da li proizvod već postoji u korpi
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == item.ProductId);

            if (existingItem != null)
            {
                // Ažuriraj količinu postojeće stavke
                existingItem.Quantity += item.Quantity;
                existingItem.Price = item.Price; // Ažuriraj cijenu ako se promijenila
            }
            else
            {
                // Dodaj novu stavku
                var cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            var cart = await _context.Carts.FindAsync(cartId);
            return await MapCartToResponseAsync(cart);
        }

        public async Task<CartResponse> RemoveItemFromCartAsync(int cartId, int productId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            var cart = await _context.Carts.FindAsync(cartId);
            return await MapCartToResponseAsync(cart);
        }

        public async Task<CartResponse> UpdateItemQuantityAsync(int cartId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return await RemoveItemFromCartAsync(cartId, productId);
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }

            var cart = await _context.Carts.FindAsync(cartId);
            return await MapCartToResponseAsync(cart);
        }

        private async Task<CartResponse> MapCartToResponseAsync(Cart cart)
        {
            var response = _mapper.Map<CartResponse>(cart);
            
            // Dohvati stavke korpe
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == cart.Id)
                .ToListAsync();

            response.Items = cartItems.Select(ci => new CartItemResponse
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

            response.TotalPrice = response.Items.Sum(item => item.TotalPrice);
            response.ItemCount = response.Items.Sum(item => item.Quantity);

            return response;
        }

        protected override CartResponse MapToResponse(Cart entity)
        {
            return MapCartToResponseAsync(entity).Result;
        }
    }
} 