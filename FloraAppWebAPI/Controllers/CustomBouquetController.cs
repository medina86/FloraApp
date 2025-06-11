using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FloraAppWebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CustomBouquetController : BaseCrudController<CustomBouquetResponse, CustomBouquetSearchObject, CustomBouquetUpsertRequest, CustomBouquetUpsertRequest>
    {
        private readonly ICustomBouquetService _customBouquetService;
    
        public CustomBouquetController(ICustomBouquetService customBouquetService) : base(customBouquetService)
        {
            _customBouquetService = customBouquetService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<PagedResult<CustomBouquetResponse>>> GetByUserId(int userId)
        {
            try
            {
                var bouquets = await _customBouquetService.GetByUserIdAsync(userId);
                return Ok(bouquets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
} 