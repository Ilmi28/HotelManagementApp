using HotelManagementApp.Application.CQRS.Hotel.Add;
using HotelManagementApp.Application.CQRS.Hotel.Delete;
using HotelManagementApp.Application.CQRS.Hotel.GetAll;
using HotelManagementApp.Application.CQRS.Hotel.GetById;
using HotelManagementApp.Application.CQRS.Hotel.Update;
using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/hotel")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    Roles = "Staff, Manager, Admin")]
[ApiController]
public class HotelController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all hotels (staff and above)
    /// </summary>
    [HttpGet("get-all")]
    [ProducesResponseType(typeof(ICollection<HotelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllHotels(CancellationToken ct)
    {
        var hotels = await mediator.Send(new GetAllHotelsQuery(), ct);
        return Ok(hotels);
    }


    /// <summary>
    /// Add hotel (staff and above)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddHotel(AddHotelCommand cmd)
    {
        await mediator.Send(cmd);
        return Created();
    }

    /// <summary>
    /// Delete hotel by id (staff and above)
    /// </summary>
    [HttpDelete("{hotelId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveHotel(int hotelId, CancellationToken ct)
    {
        await mediator.Send(new DeleteHotelCommand { HotelId = hotelId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Get hotel by id (staff and above)
    /// </summary>
    [HttpGet("{hotelId}")]
    [ProducesResponseType(typeof(HotelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelById(int hotelId, CancellationToken ct)
    {
        var hotel = await mediator.Send(new GetHotelByIdQuery { HotelId = hotelId }, ct);
        return Ok(hotel);
    }

    /// <summary>
    /// Update hotel (staff and above)
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotel([FromBody] UpdateHotelCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }
}
