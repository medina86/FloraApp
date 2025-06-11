using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FloraAppWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : BaseCrudController<
        CartResponse,
        CartSearchObject,
        CartInsertRequest,
        CartUpdateRequest>
    {
        private readonly ICartService _cartService;

        public CartController(ICartService service) : base(service)
        {
            _cartService = service;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CartResponse>> GetUserCart(int userId)
        {
            try
            {
                var result = await _cartService.GetUserCartAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("{cartId}/add-item")]
        public async Task<ActionResult<CartResponse>> AddItemToCart(int cartId, [FromBody] CartItemInsertRequest item)
        {
            try
            {
                var result = await _cartService.AddItemToCartAsync(cartId, item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("{cartId}/remove-item/{productId}")]
        public async Task<ActionResult<CartResponse>> RemoveItemFromCart(int cartId, int productId)
        {
            try
            {
                var result = await _cartService.RemoveItemFromCartAsync(cartId, productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("{cartId}/update-quantity/{productId}")]
        public async Task<ActionResult<CartResponse>> UpdateItemQuantity(int cartId, int productId, [FromQuery] int quantity)
        {
            try
            {
                var result = await _cartService.UpdateItemQuantityAsync(cartId, productId, quantity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("{cartId}/clear")]
        public async Task<ActionResult<CartResponse>> ClearCart(int cartId)
        {
            try
            {
                var result = await _cartService.ClearCartAsync(cartId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("{cartId}/total")]
        public async Task<ActionResult<decimal>> GetCartTotal(int cartId)
        {
            try
            {
                var result = await _cartService.CalculateCartTotalAsync(cartId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
} 