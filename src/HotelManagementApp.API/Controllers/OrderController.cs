using HotelManagementApp.Application.CQRS.OrderOps.CancelOrder;
using HotelManagementApp.Application.CQRS.OrderOps.ConfirmOrder;
using HotelManagementApp.Application.CQRS.OrderOps.CreateOrder;
using HotelManagementApp.Application.CQRS.OrderOps.GetOrderById;
using HotelManagementApp.Application.CQRS.OrderOps.GetOrdersByGuest;
using HotelManagementApp.Application.CQRS.OrderOps.UpdateOrder;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Models.OrderModels;
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
    /// <summary>
    /// Creates a new order
    /// </summary>
    /// <response code="204">Order created successfully</response>
    /// <response code="403">User is not authorized to create this order</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddOrder([FromBody] CreateOrderCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "AccountOwner");
        var managerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !managerPolicy.Succeeded)
            return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Updates an existing order
    /// </summary>
    /// <response code="204">Order updated successfully</response>
    /// <response code="403">User is not authorized to update this order</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Gets all orders for a specific guest
    /// </summary>
    /// <response code="200">Returns list of orders</response>
    /// <response code="403">User is not authorized to view these orders</response>
    [HttpGet("get-by-guest/{guestId}")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOrdersByGuestId(string guestId, IAuthorizationService authService, CancellationToken ct)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, guestId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, guestId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetOrdersByGuestQuery {GuestId = guestId}, ct);
        return Ok(response);
    }

    /// <summary>
    /// Gets an order by its ID
    /// </summary>
    /// <response code="200">Returns the requested order</response>
    /// <response code="403">User is not authorized to view this order</response>
    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOrderById(int orderId, IAuthorizationService authService, CancellationToken ct)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, orderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetOrderByIdQuery {OrderId = orderId}, ct);
        return Ok(response);
    }

    [HttpPatch("confirm/{orderId}")]
    public async Task<IActionResult> ConfirmOrder(int orderId, IAuthorizationService authService, CancellationToken ct)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, orderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        await mediator.Send(new ConfirmOrderCommand {OrderId = orderId}, ct);
        return NoContent();
    }
    
    [HttpPatch("cancel/{orderId}")]
    public async Task<IActionResult> CancelOrder(int orderId, IAuthorizationService authService, CancellationToken ct)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, orderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        await mediator.Send(new CancelOrderCommand {OrderId = orderId}, ct);
        return NoContent();
    }
}