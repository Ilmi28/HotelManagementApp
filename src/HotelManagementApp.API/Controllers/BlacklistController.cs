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
[Authorize]
public class BlacklistController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Adds a guest to the blacklist (manager or above)
    /// </summary>
    /// <response code="204">User added to blacklist successfully</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User does not have required permissions</response>
    /// <response code="404">User with provided id not found</response>
    /// <response code="409">User is already blacklisted</response>
    [HttpPatch("add/{userId}")]
    [Authorize(Roles = "Manager, Admin")]
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
    /// Removes a guest from the blacklist (manager or above)
    /// </summary>
    /// <response code="204">User removed from blacklist successfully</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User does not have required permissions</response>
    /// <response code="404">User with provided id not found</response>
    /// <response code="409">User is not blacklisted</response>
    [HttpPatch("remove/{userId}")]
    [Authorize(Roles = "Manager, Admin")]
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
    /// Returns all blacklisted users (staff or above)
    /// </summary>
    /// <response code="200">Returns list of blacklisted users</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User does not have required permissions</response>
    [HttpGet("all")]
    [Authorize(Roles = "Staff, Manager, Admin")]
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
