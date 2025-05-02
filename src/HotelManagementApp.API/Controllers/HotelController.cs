using HotelManagementApp.Application.CQRS.Hotel.Add;
using HotelManagementApp.Application.CQRS.Hotel.Delete;
using HotelManagementApp.Application.CQRS.Hotel.GetAll;
using HotelManagementApp.Application.CQRS.Hotel.GetById;
using HotelManagementApp.Application.CQRS.Hotel.Update;
using HotelManagementApp.Application.CQRS.Hotel.UpdateHotelImages;
using HotelManagementApp.Application.CQRS.HotelServiceOps.Add;
using HotelManagementApp.Application.CQRS.HotelServiceOps.Delete;
using HotelManagementApp.Application.CQRS.HotelServiceOps.GetByHotel;
using HotelManagementApp.Application.CQRS.HotelServiceOps.Update;
using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/hotel")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class HotelController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all hotels
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

    [HttpPost("services")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddHotelService([FromBody] AddHotelServiceCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return Created();
    }

    [HttpPut("services")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateHotelService([FromBody] UpdateHotelServiceCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpDelete("services/{hotelServiceId}")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteHotelService(int hotelServiceId, CancellationToken ct)
    {
        await mediator.Send(new DeleteHotelServiceCommand { HotelServiceId = hotelServiceId }, ct);
        return NoContent();
    }

    [HttpGet("services/{hotelId}")]
    [ProducesResponseType(typeof(ICollection<HotelServiceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHotelServices(int hotelId, CancellationToken ct)
    {
        var hotelServices = await mediator.Send(new GetHotelServicesByHotelIdQuery { HotelId = hotelId }, ct);
        return Ok(hotelServices);
    }
}
