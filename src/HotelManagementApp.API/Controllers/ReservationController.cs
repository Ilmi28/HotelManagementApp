using HotelManagementApp.Application.CQRS.OrderOps.GetOrderReservations;
using HotelManagementApp.Application.CQRS.ReservationOps.AddReservation;
using HotelManagementApp.Application.CQRS.ReservationOps.AddReservationParking;
using HotelManagementApp.Application.CQRS.ReservationOps.AddReservationService;
using HotelManagementApp.Application.CQRS.ReservationOps.GetAvailableDays;
using HotelManagementApp.Application.CQRS.ReservationOps.GetReservationParkings;
using HotelManagementApp.Application.CQRS.ReservationOps.GetReservationServices;
using HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservation;
using HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservationService;
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
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetReservationsByOrder(int orderId, IAuthorizationService authService, CancellationToken ct)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, orderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetOrderReservationsQuery {OrderId = orderId}, ct);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddReservation([FromBody] AddReservationCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, cmd.OrderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpDelete("remove/{reservationId}")]
    public async Task<IActionResult> RemoveReservation(int reservationId,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, reservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        await mediator.Send(new RemoveReservationCommand { ReservationId = reservationId}, ct);
        return NoContent();
    }

    [HttpPost("parking/add")]
    public async Task<IActionResult> AddReservationParking([FromBody] AddReservationParkingCommand cmd,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, cmd.ReservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpPost("parking/remove")]
    public async Task<IActionResult> RemoveReservationParking([FromBody] RemoveReservationCommand cmd,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, cmd.ReservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }
    
    [HttpPost("service")]
    public async Task<IActionResult> AddReservationService([FromBody] AddReservationServiceCommand cmd,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, cmd.ReservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }
    
    [HttpPost("service/remove")]
    public async Task<IActionResult> RemoveReservationService([FromBody] RemoveReservationServiceCommand cmd,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, cmd.ReservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }
    
    [HttpGet("parking/{reservationId}")]
    public async Task<IActionResult> GetReservationParkings(int reservationId,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, reservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetReservationParkingsQuery { ReservationId = reservationId }, ct);
        return Ok(response);
    }
    
    [HttpGet("service/{reservationId}")]
    public async Task<IActionResult> GetReservationServices(int reservationId,
        IAuthorizationService authService, CancellationToken ct)
    {
        var reservationPolicy = await authService.AuthorizeAsync(User, reservationId, "ReservationAccess");
        if (!reservationPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetReservationServicesQuery { ReservationId = reservationId }, ct);
        return Ok(response);
    }

    [HttpPost("get-available-days")]
    public async Task<IActionResult> GetAvailableDays([FromBody] GetAvailableDaysQuery query, CancellationToken ct)
    {
        var response = await mediator.Send(query, ct);
        return Ok(response);
    }
}