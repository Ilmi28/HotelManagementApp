using HotelManagementApp.Application.CQRS.Blacklist.Add;
using HotelManagementApp.Application.CQRS.Blacklist.GetAll;
using HotelManagementApp.Application.CQRS.Blacklist.Remove;
using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/blacklist")]
[ApiController]
[Authorize(Roles = "Manager, Admin")]
public class BlacklistController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Adds a guest to the blacklist (manager and above).
    /// </summary>
    [HttpPatch("add/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddToBlacklist(string userId, CancellationToken ct)
    {
        await mediator.Send(new AddToBlacklistCommand { UserId = userId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Removes a guest from the blacklist (manager and above).
    /// </summary>
    [HttpPatch("remove/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveFromBlacklist(string userId, CancellationToken ct)
    {
        await mediator.Send(new RemoveFromBlacklistCommand { UserId = userId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns all blacklisted users (manager and above).
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ICollection<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllBlacklistedUsers(CancellationToken ct)
    {
        var query = new GetBlacklistQuery();
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }
}
