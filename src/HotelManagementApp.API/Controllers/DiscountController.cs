using HotelManagementApp.Application.CQRS.Discount.AddHotelDiscount;
using HotelManagementApp.Application.CQRS.Discount.AddParkingDiscount;
using HotelManagementApp.Application.CQRS.Discount.AddRoomDiscount;
using HotelManagementApp.Application.CQRS.Discount.AddServiceDiscount;
using HotelManagementApp.Application.CQRS.Discount.GetDiscountsByHotel;
using HotelManagementApp.Application.CQRS.Discount.GetDiscountsByParking;
using HotelManagementApp.Application.CQRS.Discount.GetDiscountsByRoom;
using HotelManagementApp.Application.CQRS.Discount.GetDiscountsByService;
using HotelManagementApp.Application.CQRS.Discount.RemoveHotelDiscount;
using HotelManagementApp.Application.CQRS.Discount.RemoveParkingDiscount;
using HotelManagementApp.Application.CQRS.Discount.RemoveRoomDiscount;
using HotelManagementApp.Application.CQRS.Discount.RemoveServiceDiscount;
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
    
    /// <summary>
    /// Adds a new discount for a hotel (manager or above)
    /// </summary>
    /// <response code="204">Discount added successfully</response>
    /// <response code="403">User is unauthorized to add discounts</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost("hotel")]
    public async Task<IActionResult> AddHotelDiscount([FromBody] AddHotelDiscountCommand command, CancellationToken ct)
    {
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Adds a new discount for a room (manager or above)
    /// </summary>
    /// <response code="204">Discount added successfully</response>
    /// <response code="403">User is unauthorized to add discounts</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost("room")]
    public async Task<IActionResult> AddRoomDiscount([FromBody] AddRoomDiscountCommand command, CancellationToken ct)
    {
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Adds a new discount for a parking spot (manager or above)
    /// </summary>
    /// <response code="204">Discount added successfully</response>
    /// <response code="403">User is unauthorized to add discounts</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost("parking")]
    public async Task<IActionResult> AddParkingDiscount([FromBody] AddParkingDiscountCommand command, CancellationToken ct)
    {
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Adds a new discount for a service (manager or above)
    /// </summary>
    /// <response code="204">Discount added successfully</response>
    /// <response code="403">User is unauthorized to add discounts</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost("service")]
    public async Task<IActionResult> AddServiceDiscount([FromBody] AddServiceDiscountCommand command, CancellationToken ct)
    {
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Returns all discounts for a specific hotel (manager or above)
    /// </summary>
    /// <response code="200">Returns list of hotel discounts</response>
    /// <response code="403">User is unauthorized to view discounts</response>
    [ProducesResponseType(typeof(ICollection<HotelDiscountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("hotel/{id}")]
    public async Task<IActionResult> GetHotelDiscount(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDiscountByHotelQuery { HotelId = id }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Returns all discounts for a specific room (manager or above)
    /// </summary>
    /// <response code="200">Returns list of room discounts</response>
    /// <response code="403">User is unauthorized to view discounts</response>
    [ProducesResponseType(typeof(ICollection<RoomDiscountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("room/{id}")]
    public async Task<IActionResult> GetRoomDiscount(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDiscountsByRoomQuery { RoomId = id }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Returns all discounts for a specific parking spot (manager or above)
    /// </summary>
    /// <response code="200">Returns list of parking discounts</response>
    /// <response code="403">User is unauthorized to view discounts</response>
    [ProducesResponseType(typeof(ICollection<ParkingDiscountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("parking/{id}")]
    public async Task<IActionResult> GetParkingDiscount(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDiscountsByParkingQuery { ParkingId = id }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Returns all discounts for a specific service (manager or above)
    /// </summary>
    /// <response code="200">Returns list of service discounts</response>
    /// <response code="403">User is unauthorized to view discounts</response>
    [ProducesResponseType(typeof(ICollection<ServiceDiscountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("service/{id}")]
    public async Task<IActionResult> GetServiceDiscount(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDiscountsByServiceQuery { ServiceId = id }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a hotel discount (manager or above)
    /// </summary>
    /// <response code="204">Discount deleted successfully</response>
    /// <response code="403">User is unauthorized to delete discounts</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpDelete("hotel/{id}")]
    public async Task<IActionResult> DeleteHotelDiscount(int id, CancellationToken ct)
    {
        await mediator.Send(new RemoveHotelDiscountCommand { DiscountId = id }, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a room discount (manager or above)
    /// </summary>
    /// <response code="204">Discount deleted successfully</response>
    /// <response code="403">User is unauthorized to delete discounts</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpDelete("room/{id}")]
    public async Task<IActionResult> DeleteRoomDiscount(int id, CancellationToken ct)
    {
        await mediator.Send(new RemoveRoomDiscountCommand { DiscountId = id }, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a parking discount (manager or above)
    /// </summary>
    /// <response code="204">Discount deleted successfully</response>
    /// <response code="403">User is unauthorized to delete discounts</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpDelete("parking/{id}")]
    public async Task<IActionResult> DeleteParkingDiscount(int id, CancellationToken ct)
    {
        await mediator.Send(new RemoveParkingDiscountCommand { DiscountId = id }, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a service discount (manager or above)
    /// </summary>
    /// <response code="204">Discount deleted successfully</response>
    /// <response code="403">User is unauthorized to delete discounts</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpDelete("service/{id}")]
    public async Task<IActionResult> DeleteServiceDiscount(int id, CancellationToken ct)
    {
        await mediator.Send(new RemoveServiceDiscountCommand { DiscountId = id }, ct);
        return NoContent();
    }
}
