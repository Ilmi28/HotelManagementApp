using HotelManagementApp.Application.CQRS.HotelOps.Add;
using HotelManagementApp.Application.CQRS.HotelOps.Delete;
using HotelManagementApp.Application.CQRS.HotelOps.GetAll;
using HotelManagementApp.Application.CQRS.HotelOps.GetById;
using HotelManagementApp.Application.CQRS.HotelOps.GetWeatherByHotelId;
using HotelManagementApp.Application.CQRS.HotelOps.Update;
using HotelManagementApp.Application.CQRS.HotelOps.UpdateHotelImages;
using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/hotel")]
[Authorize]
[ApiController]
public class HotelController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all hotels
    /// </summary>
    [HttpGet("get-all")]
    [AllowAnonymous]
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
    [Authorize(Roles = "Admin, Manager, Staff")]
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
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveHotel(int hotelId, CancellationToken ct)
    {
        await mediator.Send(new DeleteHotelCommand { HotelId = hotelId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Get hotel by id
    /// </summary>
    [HttpGet("{hotelId}")]
    [AllowAnonymous]
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
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotel([FromBody] UpdateHotelCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Updates hotel images (staff and above)
    /// </summary>
    [HttpPut("images")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateHotelImages([FromForm] UpdateHotelImagesCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpGet("weather/{hotelId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(WeatherResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWeatherByHotelId(int hotelId, CancellationToken ct)
    {
        var response = await mediator.Send(new GetWeatherByHotelIdQuery { HotelId = hotelId }, ct);
        return Ok(response);
    }
}