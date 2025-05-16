using HotelManagementApp.Application.CQRS.OrderOps.GetOrderReservations;
using HotelManagementApp.Application.CQRS.ReservationOps.AddReservation;
using HotelManagementApp.Application.CQRS.ReservationOps.AddReservationParking;
using HotelManagementApp.Application.CQRS.ReservationOps.AddReservationService;
using HotelManagementApp.Application.CQRS.ReservationOps.GetAvailableDays;
using HotelManagementApp.Application.CQRS.ReservationOps.GetReservationParkings;
using HotelManagementApp.Application.CQRS.ReservationOps.GetReservationServices;
using HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservation;
using HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservationService;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Models.OrderModels;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/reservation")]
[Authorize]
[ApiController]
public class ReservationController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets all reservations by order ID.
    /// </summary>
    /// <response code="200">Returns reservations</response>
    /// <response code="403">Access denied</response>
    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(ICollection<ReservationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetReservationsByOrder(int orderId, IAuthorizationService authService, CancellationToken ct)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, orderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetOrderReservationsQuery { OrderId = orderId }, ct);
        return Ok(response);
    }

    /// <summary>
    /// Adds a new reservation to an order.
    /// </summary>
    /// <response code="204">Added successfully</response>
    /// <response code="403">Access denied</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddReservation([FromBody] AddReservationCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, cmd.OrderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Removes a reservation.
    /// </summary>
    /// <response code="204">Removed successfully</response>
    /// <response code="403">Access denied</response>
    [HttpDelete("remove/{reservationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveReservation(int reservationId,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, reservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        await mediator.Send(new RemoveReservationCommand { ReservationId = reservationId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Adds parking to a reservation.
    /// </summary>
    /// <response code="204">Added successfully</response>
    /// <response code="403">Access denied</response>
    [HttpPost("parking/add")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddReservationParking([FromBody] AddReservationParkingCommand cmd,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, cmd.ReservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Removes parking from a reservation.
    /// </summary>
    /// <response code="204">Removed successfully</response>
    /// <response code="403">Access denied</response>
    [HttpPost("parking/remove")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveReservationParking([FromBody] RemoveReservationCommand cmd,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, cmd.ReservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Adds a service to a reservation.
    /// </summary>
    /// <response code="204">Added successfully</response>
    /// <response code="403">Access denied</response>
    [HttpPost("service")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddReservationService([FromBody] AddReservationServiceCommand cmd,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, cmd.ReservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Removes a service from a reservation.
    /// </summary>
    /// <response code="204">Removed successfully</response>
    /// <response code="403">Access denied</response>
    [HttpPost("service/remove")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveReservationService([FromBody] RemoveReservationServiceCommand cmd,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, cmd.ReservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Gets all parkings for a reservation.
    /// </summary>
    /// <response code="200">Returns parkings</response>
    /// <response code="403">Access denied</response>
    [HttpGet("parking/{reservationId}")]
    [ProducesResponseType(typeof(ICollection<ReservationParkingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetReservationParkings(int reservationId,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, reservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetReservationParkingsQuery { ReservationId = reservationId }, ct);
        return Ok(response);
    }

    /// <summary>
    /// Gets all services for a reservation.
    /// </summary>
    /// <response code="200">Returns services</response>
    /// <response code="403">Access denied</response>
    [HttpGet("service/{reservationId}")]
    [ProducesResponseType(typeof(ICollection<ReservationServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetReservationServices(int reservationId,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, reservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetReservationServicesQuery { ReservationId = reservationId }, ct);
        return Ok(response);
    }

    /// <summary>
    /// Gets available days for reservation based on request.
    /// </summary>
    /// <response code="200">Returns available days</response>
    [HttpPost("get-available-days")]
    [ProducesResponseType(typeof(ICollection<DateOnly>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableDays([FromBody] GetAvailableDaysQuery query, CancellationToken ct)
    {
        var response = await mediator.Send(query, ct);
        return Ok(response);
    }
}