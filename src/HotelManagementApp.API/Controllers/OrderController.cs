using HotelManagementApp.Application.CQRS.OrderOps.CreateOrder;
using HotelManagementApp.Application.CQRS.OrderOps.GetOrdersByGuest;
using HotelManagementApp.Application.CQRS.OrderOps.UpdateOrder;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/order")]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddOrder([FromBody] CreateOrderCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "AccountOwner");
        var managerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !managerPolicy.Succeeded)
            return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpGet("get-by-guest/{guestId}")]
    public async Task<IActionResult> GetOrdersByGuestId(string guestId, IAuthorizationService authService, CancellationToken ct)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, guestId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, guestId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetOrdersByGuestQuery {GuestId = guestId}, ct);
        return Ok(response);
    }
}