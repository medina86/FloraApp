using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using FloraApp.Model.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FloraAppWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : BaseCrudController<
        ReservationResponse,
        ReservationSearchObject,
        ReservationInsertRequest,
        ReservationUpdateRequest>
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService service) : base(service)
        {
            _reservationService = service;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ReservationResponse>>> GetUserReservations(int userId)
        {
            try
            {
                var result = await _reservationService.GetUserReservationsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("pending")]
        public async Task<ActionResult<List<ReservationResponse>>> GetPendingReservations()
        {
            try
            {
                var result = await _reservationService.GetPendingReservationsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<List<ReservationResponse>>> GetReservationsByDateRange(
            [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            try
            {
                var result = await _reservationService.GetReservationsByDateRangeAsync(from, to);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("{id}/with-proposals")]
        public async Task<ActionResult<ReservationResponse>> GetReservationWithProposals(int id)
        {
            try
            {
                var result = await _reservationService.GetReservationWithProposalsAsync(id);
                return Ok(result);
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

        [HttpPut("{id}/status")]
        public async Task<ActionResult<ReservationResponse>> UpdateReservationStatus(
            int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                var result = await _reservationService.UpdateReservationStatusAsync(id, request.Status, request.AdminNotes);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("{id}/allowed-transitions")]
        public async Task<ActionResult<List<ReservationStatus>>> GetAllowedTransitions(int id)
        {
            try
            {
                var result = await _reservationService.GetAllowedTransitionsAsync(id);
                return Ok(result);
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

        [HttpGet("{id}/status-description")]
        public async Task<ActionResult<string>> GetStatusDescription(int id)
        {
            try
            {
                var result = await _reservationService.GetStatusDescriptionAsync(id);
                return Ok(result);
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

    public class UpdateStatusRequest
    {
        public string Status { get; set; }
        public string? AdminNotes { get; set; }
    }
} 