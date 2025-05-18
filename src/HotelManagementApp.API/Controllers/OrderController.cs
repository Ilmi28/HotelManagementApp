using HotelManagementApp.Application.CQRS.OrderOps.CancelOrder;
using HotelManagementApp.Application.CQRS.OrderOps.ConfirmOrder;
using HotelManagementApp.Application.CQRS.OrderOps.CreateOrder;
using HotelManagementApp.Application.CQRS.OrderOps.GetBillPdf;
using HotelManagementApp.Application.CQRS.OrderOps.GetOrderById;
using HotelManagementApp.Application.CQRS.OrderOps.GetOrdersByGuest;
using HotelManagementApp.Application.CQRS.OrderOps.UpdateOrder;
using HotelManagementApp.Application.CQRS.OrderOps.GetPendingOrders;
using HotelManagementApp.Application.CQRS.OrderOps.GetConfirmedOrders;
using HotelManagementApp.Application.CQRS.OrderOps.GetCompletedOrders;
using HotelManagementApp.Application.CQRS.OrderOps.GetCancelledOrders;
using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;
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
    /// <response code="200">Order created successfully and returns order id</response>
    /// <response code="403">User is not authorized to create this order</response>
    [HttpPost]
    [Authorize(Policy = "EmailConfirmed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddOrder([FromBody] CreateOrderCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "AccountOwner");
        var managerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !managerPolicy.Succeeded)
            return Forbid();
        var response = await mediator.Send(cmd, ct);
        return Ok(new { OrderId = response });
    }

    /// <summary>
    /// Updates an existing order
    /// </summary>
    /// <response code="204">Order updated successfully</response>
    /// <response code="403">User is not authorized to update this order</response>
    [HttpPut]
    [Authorize(Policy = "EmailConfirmed")]
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
    [Authorize(Policy = "EmailConfirmed")]
    public async Task<IActionResult> ConfirmOrder(int orderId, IAuthorizationService authService, CancellationToken ct)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, orderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        await mediator.Send(new ConfirmOrderCommand {OrderId = orderId}, ct);
        return NoContent();
    }
    
    [HttpPatch("cancel/{orderId}")]
    [Authorize(Policy = "EmailConfirmed")]
    public async Task<IActionResult> CancelOrder(int orderId, IAuthorizationService authService, CancellationToken ct)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, orderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        await mediator.Send(new CancelOrderCommand {OrderId = orderId}, ct);
        return NoContent();
    }
    
    /// <summary>
    /// Gets all pending orders
    /// </summary>
    /// <response code="200">Returns list of pending orders</response>
    /// <response code="403">User is not authorized for viewing orders</response>
    [HttpGet("pending")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingOrders(CancellationToken ct)
    {
        var response = await mediator.Send(new GetPendingOrdersQuery(), ct);
        return Ok(response);
    }
    
    /// <summary>
    /// Gets all confirmed orders
    /// </summary>
    /// <response code="200">Returns list of confirmed orders</response>
    /// <response code="403">User is not authorized for viewing orders</response>
    [HttpGet("confirmed")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConfirmedOrders(CancellationToken ct)
    {
        var response = await mediator.Send(new GetConfirmedOrdersQuery(), ct);
        return Ok(response);
    }
    
    /// <summary>
    /// Gets all completed orders
    /// </summary>
    /// <response code="200">Returns list of completed orders</response>
    /// <response code="403">User is not authorized for viewing orders</response>
    [HttpGet("completed")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompletedOrders(CancellationToken ct)
    {
        var response = await mediator.Send(new GetCompletedOrdersQuery(), ct);
        return Ok(response);
    }
    
    /// <summary>
    /// Gets all cancelled orders
    /// </summary>
    /// <response code="200">Returns list of cancelled orders</response>
    /// <response code="403">User is not authorized for viewing orders</response>
    [HttpGet("cancelled")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCancelledOrders(CancellationToken ct)
    {
        var response = await mediator.Send(new GetCancelledOrdersQuery(), ct);
        return Ok(response);
    }
    
    /// <summary>
    /// Returns bill for order in pdf format
    /// </summary>
    /// <response code="200">Returns bill for order</response>
    [HttpGet("{orderId}/bill")]
    [ProducesResponseType(typeof(File), StatusCodes.Status200OK, "application/pdf")]
    public async Task<IActionResult> GetBill(int orderId, IAuthorizationService authService, CancellationToken ct)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, orderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetOrderBillPdfQuery { OrderId = orderId }, ct);
        return File(response, "application/pdf", $"Faktura_Zamowienie{orderId}.pdf");
    }
}