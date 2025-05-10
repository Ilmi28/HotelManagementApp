using HotelManagementApp.Application.CQRS.Discount.AddHotelDiscount;
using HotelManagementApp.Application.CQRS.Discount.AddRoomDiscount;
using HotelManagementApp.Application.CQRS.Discount.GetDiscountsByHotel;
using HotelManagementApp.Application.CQRS.Discount.RemoveHotelDiscount;
using HotelManagementApp.Application.Responses.DiscountResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/discounts")]
[Authorize(Roles = "Manager, Admin")]
[ApiController]
public class DiscountController(IMediator mediator) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("hotel")]
    public async Task<IActionResult> AddHotelDiscount([FromBody] AddHotelDiscountCommand command, CancellationToken ct)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("room")]
    public async Task<IActionResult> AddRoomDiscount([FromBody] AddRoomDiscountCommand command, CancellationToken ct)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("parking")]
    public async Task<IActionResult> AddParkingDiscount([FromBody] AddRoomDiscountCommand command, CancellationToken ct)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("service")]
    public async Task<IActionResult> AddServiceDiscount([FromBody] AddRoomDiscountCommand command, CancellationToken ct)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [ProducesResponseType(typeof(ICollection<HotelDiscountResponse>), StatusCodes.Status200OK)]
    [HttpGet("hotel/{id}")]
    public async Task<IActionResult> GetHotelDiscount(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDiscountByHotelQuery { HotelId = id }, ct);
        return Ok(result);
    }

    [ProducesResponseType(typeof(ICollection<RoomDiscountResponse>), StatusCodes.Status200OK)]
    [HttpGet("room/{id}")]
    public async Task<IActionResult> GetRoomDiscount(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDiscountByHotelQuery { HotelId = id }, ct);
        return Ok(result);
    }

    [ProducesResponseType(typeof(ICollection<ParkingDiscountResponse>), StatusCodes.Status200OK)]
    [HttpGet("parking/{id}")]
    public async Task<IActionResult> GetParkingDiscount(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDiscountByHotelQuery { HotelId = id }, ct);
        return Ok(result);
    }

    [ProducesResponseType(typeof(ICollection<ServiceDiscountResponse>), StatusCodes.Status200OK)]
    [HttpGet("service/{id}")]
    public async Task<IActionResult> GetServiceDiscount(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDiscountByHotelQuery { HotelId = id }, ct);
        return Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("hotel/{id}")]
    public async Task<IActionResult> DeleteHotelDiscount(int id, CancellationToken ct)
    {
        await mediator.Send(new RemoveHotelDiscountCommand { DiscountId = id }, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("room/{id}")]
    public async Task<IActionResult> DeleteRoomDiscount(int id, CancellationToken ct)
    {
        await mediator.Send(new RemoveHotelDiscountCommand { DiscountId = id }, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("parking/{id}")]
    public async Task<IActionResult> DeleteParkingDiscount(int id, CancellationToken ct)
    {
        await mediator.Send(new RemoveHotelDiscountCommand { DiscountId = id }, ct);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("service/{id}")]
    public async Task<IActionResult> DeleteServiceDiscount(int id, CancellationToken ct)
    {
        await mediator.Send(new RemoveHotelDiscountCommand { DiscountId = id }, ct);
        return NoContent();
    }
}
