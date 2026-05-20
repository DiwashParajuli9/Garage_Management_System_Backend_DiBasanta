using GarageManagement.API.Models.DTOs.PartRequests;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/part-requests")]
[Authorize(Roles = "Staff,Admin,Customer")]
public class PartRequestsController : ControllerBase
{
    private readonly IPartRequestService _partRequestService;

    public PartRequestsController(IPartRequestService partRequestService)
    {
        _partRequestService = partRequestService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePartRequest([FromBody] CreatePartRequestDto dto)
    {
        try
        {
            var partRequest = await _partRequestService.CreatePartRequestAsync(dto);
            return Created($"api/part-requests/{partRequest.Id}", partRequest);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPartRequests([FromQuery] string? status = null, [FromQuery] string? searchTerm = null)
    {
        try
        {
            var requests = await _partRequestService.GetPartRequestsAsync(status, searchTerm);
            return Ok(requests);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{partRequestId:guid}")]
    public async Task<IActionResult> GetPartRequestById(Guid partRequestId)
    {
        try
        {
            var request = await _partRequestService.GetPartRequestByIdAsync(partRequestId);
            return Ok(request);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{partRequestId:guid}/history")]
    public async Task<IActionResult> GetPartRequestCommentHistory(Guid partRequestId)
    {
        try
        {
            var history = await _partRequestService.GetPartRequestCommentHistoryAsync(partRequestId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{partRequestId:guid}")]
    public async Task<IActionResult> UpdatePartRequest(Guid partRequestId, [FromBody] UpdatePartRequestDto dto)
    {
        try
        {
            var request = await _partRequestService.UpdatePartRequestAsync(partRequestId, dto);
            return Ok(request);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{partRequestId:guid}")]
    public async Task<IActionResult> DeletePartRequest(Guid partRequestId)
    {
        try
        {
            await _partRequestService.DeletePartRequestAsync(partRequestId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
