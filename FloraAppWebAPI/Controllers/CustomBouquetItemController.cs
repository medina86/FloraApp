using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloraAppWebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CustomBouquetItemController : ControllerBase
    {
        private readonly ICustomBouquetItemService _itemService;

        public CustomBouquetItemController(ICustomBouquetItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("bouquet/{bouquetId}")]
        public async Task<ActionResult<List<CustomBouquetItemResponse>>> GetByBouquetId(int bouquetId)
        {
            try
            {
                var items = await _itemService.GetByBouquetIdAsync(bouquetId);
                return Ok(items);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("bouquet/{bouquetId}")]
        public async Task<ActionResult<CustomBouquetItemResponse>> AddItem(int bouquetId, [FromBody] CustomBouquetItemUpsertRequest request)
        {
            try
            {
                var item = await _itemService.AddItemAsync(bouquetId, request);
                return CreatedAtAction(nameof(GetByBouquetId), new { bouquetId }, item);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("{itemId}")]
        public async Task<ActionResult<CustomBouquetItemResponse>> UpdateItem(int itemId, [FromBody] CustomBouquetItemUpsertRequest request)
        {
            try
            {
                var item = await _itemService.UpdateItemAsync(itemId, request);
                return Ok(item);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("{itemId}")]
        public async Task<ActionResult> DeleteItem(int itemId)
        {
            try
            {
                await _itemService.DeleteItemAsync(itemId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
} 