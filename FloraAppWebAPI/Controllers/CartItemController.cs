using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FloraAppWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : BaseCrudController<
        CartItemResponse,
        CartItemSearchObject,
        CartItemInsertRequest,
        CartItemUpdateRequest>
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService service) : base(service)
        {
            _cartItemService = service;
        }

        [HttpGet("cart/{cartId}")]
        public async Task<ActionResult<List<CartItemResponse>>> GetCartItems(int cartId)
        {
            try
            {
                var result = await _cartItemService.GetCartItemsAsync(cartId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("cart/{cartId}/product/{productId}")]
        public async Task<ActionResult<CartItemResponse>> GetCartItemByProduct(int cartId, int productId)
        {
            try
            {
                var result = await _cartItemService.GetCartItemByProductAsync(cartId, productId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("cart/{cartId}/product/{productId}/exists")]
        public async Task<ActionResult<bool>> ItemExistsInCart(int cartId, int productId)
        {
            try
            {
                var result = await _cartItemService.ItemExistsInCartAsync(cartId, productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
} 