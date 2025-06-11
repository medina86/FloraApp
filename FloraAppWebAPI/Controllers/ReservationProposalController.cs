using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FloraAppWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationProposalController : BaseCrudController<
        ReservationProposalResponse,
        ReservationProposalSearchObject,
        ReservationProposalInsertRequest,
        ReservationProposalUpdateRequest>
    {
        private readonly IReservationProposalService _proposalService;

        public ReservationProposalController(IReservationProposalService service) : base(service)
        {
            _proposalService = service;
        }

        [HttpGet("reservation/{reservationId}")]
        public async Task<ActionResult<List<ReservationProposalResponse>>> GetProposalsForReservation(int reservationId)
        {
            try
            {
                var result = await _proposalService.GetProposalsForReservationAsync(reservationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<ReservationProposalResponse>>> GetActiveProposals()
        {
            try
            {
                var result = await _proposalService.GetActiveProposalsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("expired")]
        public async Task<ActionResult<List<ReservationProposalResponse>>> GetExpiredProposals()
        {
            try
            {
                var result = await _proposalService.GetExpiredProposalsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<ReservationProposalResponse>> UpdateProposalStatus(
            int id, [FromBody] UpdateProposalStatusRequest request)
        {
            try
            {
                var result = await _proposalService.UpdateProposalStatusAsync(id, request.Status);
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

    public class UpdateProposalStatusRequest
    {
        public string Status { get; set; }
    }
} 