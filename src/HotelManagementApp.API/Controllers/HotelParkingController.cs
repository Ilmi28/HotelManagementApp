using HotelManagementApp.Application.CQRS.HotelParkingOps.Add;
using HotelManagementApp.Application.CQRS.HotelParkingOps.Delete;
using HotelManagementApp.Application.CQRS.HotelParkingOps.GetByHotelId;
using HotelManagementApp.Application.CQRS.HotelParkingOps.GetByid;
using HotelManagementApp.Application.CQRS.HotelParkingOps.Update;
using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[ApiController]
[Route("api/hotel-parkings")]
[Authorize]
public class HotelParkingController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddHotelParking([FromBody] AddHotelParkingCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return Created();
    }

    [HttpGet("get-by-hotel/{hotelId}")]
    [ProducesResponseType(typeof(ICollection<HotelParkingResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHotelParkings(int hotelId, CancellationToken ct)
    {
        var hotelParkings = await mediator.Send(new GetHotelParkingsByHotelIdQuery { HotelId = hotelId }, ct);
        return Ok(hotelParkings);
    }

    [HttpDelete("{hotelParkingId}")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteHotelParking(int hotelParkingId, CancellationToken ct)
    {
        await mediator.Send(new DeleteHotelParkingCommand { Id = hotelParkingId }, ct);
        return NoContent();
    }

    [HttpPut]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateHotelParking([FromBody] UpdateHotelParkingCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpGet("{parkingId}")]
    public async Task<IActionResult> GetHotelParkingById(int parkingId, CancellationToken ct)
    {
        var response = await mediator.Send(new GetHotelParkingByIdQuery { ParkingId = parkingId }, ct);
        return Ok(response);
    }
}
