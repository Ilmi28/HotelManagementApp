using HotelManagementApp.Application.CQRS.VIP.Add;
using HotelManagementApp.Application.CQRS.VIP.GetAll;
using HotelManagementApp.Application.CQRS.VIP.Remove;
using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/vip")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    Roles = "Staff, Manager, Admin")]
public class VIPController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Adds a guest to the VIP list (staff and above).
    /// </summary>
    [HttpPatch("add/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddToVIP(string userId, CancellationToken ct)
    {
        await mediator.Send(new AddToVIPCommand { UserId = userId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Removes a guest from the VIP list (staff and above).
    /// </summary>
    [HttpPatch("remove/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveFromVIP(string userId, CancellationToken ct)
    {
        await mediator.Send(new RemoveFromVIPCommand { UserId = userId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns all VIP users (staff and above).
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ICollection<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllVIPUsers(CancellationToken ct)
    {
        var query = new GetVIPListQuery();
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }
}
