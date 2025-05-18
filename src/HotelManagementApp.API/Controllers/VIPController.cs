using HotelManagementApp.Application.CQRS.VIP.Add;
using HotelManagementApp.Application.CQRS.VIP.GetAll;
using HotelManagementApp.Application.CQRS.VIP.IsGuestVIP;
using HotelManagementApp.Application.CQRS.VIP.Remove;
using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/vip")]
[ApiController]
[Authorize]
public class VipController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Adds a guest to the VIP list. (manager or above)
    /// </summary>
    /// <param name="userId">ID of the user to add as VIP.</param>
    /// <response code="204">User added to VIP successfully.</response>
    /// <response code="401">Authentication required.</response>
    /// <response code="403">Access denied.</response>
    /// <response code="404">User not found.</response>
    /// <response code="409">User already VIP.</response>
    [HttpPatch("add/{userId}")]
    [Authorize(Roles = "Manager, Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddToVip(string userId, CancellationToken ct)
    {
        await mediator.Send(new AddToVIPCommand { UserId = userId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Removes a guest from the VIP list. (manager or above)
    /// </summary>
    /// <param name="userId">ID of the user to remove from VIP.</param>
    /// <response code="204">User removed from VIP successfully.</response>
    /// <response code="401">Authentication required.</response>
    /// <response code="403">Access denied.</response>
    /// <response code="404">User not found.</response>
    /// <response code="409">User not VIP.</response>
    [HttpPatch("remove/{userId}")]
    [Authorize(Roles = "Manager, Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RemoveFromVip(string userId, CancellationToken ct)
    {
        await mediator.Send(new RemoveFromVIPCommand { UserId = userId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Gets all VIP users. (staff or above)
    /// </summary>
    /// <response code="200">Returns VIP user list.</response>
    /// <response code="401">Authentication required.</response>
    /// <response code="403">Access denied.</response>
    [HttpGet("all")]
    [Authorize(Roles = "Manager, Admin, Staff")]
    [ProducesResponseType(typeof(ICollection<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllVipUsers(CancellationToken ct)
    {
        var query = new GetVipListQuery();
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }

    /// <summary>
    /// Checks if a user is a VIP. (owner or above)
    /// </summary>
    /// <param name="userId">User ID to check VIP status.</param>
    /// <response code="200">Returns true if VIP, false otherwise.</response>
    /// <response code="401">Authentication required.</response>
    /// <response code="403">Access denied.</response>
    [HttpGet("isVIP/{userId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> IsGuestVip(string userId, IAuthorizationService authService, CancellationToken ct)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, userId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, userId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();

        var query = new IsGuestVIPQuery { UserId = userId };
        var result = await mediator.Send(query, ct);
        return Ok(new { IsVIP = result });
    }
}
