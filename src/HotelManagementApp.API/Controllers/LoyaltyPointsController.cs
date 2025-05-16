using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.AddLoyaltyReward;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.ExchangePointsForReward;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetAcquiredRewardsByGuest;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetAvailableRewards;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsByGuest;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsHistoryByGuest;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsHistoryOfGuests;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetRewardById;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.RemoveLoyaltyReward;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.UpdateLoyaltyReward;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.API.Controllers;

[ApiController]
[Route("api/loyalty-points")]
[Authorize]
public class LoyaltyPointsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new loyalty reward (manager or above)
    /// </summary>
    /// <response code="200">Reward created successfully</response>
    [HttpPost("rewards")]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddLoyaltyReward([FromBody] AddLoyaltyRewardCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Exchanges loyalty points for a reward 
    /// </summary>
    /// <response code="200">Points exchanged successfully</response>
    [HttpPost("exchange")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExchangePointsForReward([FromBody] ExchangePointsForRewardCommand command,
        IAuthorizationService authService, CancellationToken cancellationToken)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, command.GuestId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, command.GuestId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Returns all rewards acquired by a specific guest (owner or above)
    /// </summary>
    /// <response code="200">Returns list of acquired rewards</response>
    [HttpGet("guests/{guestId}/acquired-rewards")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAcquiredRewardsByGuest(string guestId, IAuthorizationService authService,CancellationToken cancellationToken)
    {
        var query = new GetAcquiredRewardsByGuestIdQuery { GuestId = guestId };
        var ownerPolicy = await authService.AuthorizeAsync(User, guestId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, guestId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns all available loyalty rewards
    /// </summary>
    /// <response code="200">Returns list of available rewards</response>
    [HttpGet("rewards")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableRewards(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAvailableRewardsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns loyalty points balance for a specific guest (owner or above)
    /// </summary>
    /// <response code="200">Returns guest's loyalty points</response>
    [HttpGet("guests/{guestId}/points")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLoyaltyPointsByGuest(string guestId, IAuthorizationService authService, CancellationToken cancellationToken)
    {
        var query = new GetLoyaltyPointsByGuestIdQuery { GuestId = guestId };
        var ownerPolicy = await authService.AuthorizeAsync(User, guestId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, guestId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns loyalty points history for a specific guest (owner or above)
    /// </summary>
    /// <response code="200">Returns guest's loyalty points history</response>
    [HttpGet("guests/{guestId}/history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLoyaltyPointsHistoryByGuest(string guestId, IAuthorizationService authService, CancellationToken cancellationToken)
    {
        var query = new GetLoyaltyPointsHistoryByGuestIdQuery { GuestId = guestId };
        var ownerPolicy = await authService.AuthorizeAsync(User, guestId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, guestId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns loyalty points history for all guests (staff or above)
    /// </summary>
    /// <response code="200">Returns loyalty points history for all guests</response>
    [HttpGet("guests/points")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLoyaltyPointsOfGuests(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetLoyaltyPointsHistoryOfGuestsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns a specific loyalty reward by ID
    /// </summary>
    /// <response code="200">Returns the requested reward</response>
    [HttpGet("rewards/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRewardById(int id, CancellationToken cancellationToken)
    {
        var query = new GetLoyaltyRewardByIdQuery { LoyaltyRewardId = id };
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Removes a specific loyalty reward (manager or above)
    /// </summary>
    /// <response code="204">Reward removed successfully</response>
    [HttpDelete("rewards/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveLoyaltyReward(int id, CancellationToken cancellationToken)
    {
        var command = new RemoveLoyaltyRewardCommand { LoyaltyRewardId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Updates a specific loyalty reward (manager or above)
    /// </summary>
    /// <response code="204">Reward updated successfully</response>
    [HttpPut("rewards/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateLoyaltyReward([FromBody] UpdateLoyaltyRewardCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}