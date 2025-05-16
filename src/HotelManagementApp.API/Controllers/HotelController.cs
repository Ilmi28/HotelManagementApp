using HotelManagementApp.Application.CQRS.HotelOps.Add;
using HotelManagementApp.Application.CQRS.HotelOps.Delete;
using HotelManagementApp.Application.CQRS.HotelOps.GetAll;
using HotelManagementApp.Application.CQRS.HotelOps.GetById;
using HotelManagementApp.Application.CQRS.HotelOps.GetWeatherByHotelId;
using HotelManagementApp.Application.CQRS.HotelOps.Update;
using HotelManagementApp.Application.CQRS.HotelOps.UpdateHotelImages;
using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/hotel")]
[Authorize]
[ApiController]
public class HotelController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Returns all hotels in the system 
    /// </summary>
    /// <response code="200">Returns list of all hotels</response>
    /// <response code="401">User is not authenticated (when authorization is required)</response>
    [HttpGet("get-all")]
    [ProducesResponseType(typeof(ICollection<HotelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllHotels(CancellationToken ct)
    {
        var hotels = await mediator.Send(new GetAllHotelsQuery(), ct);
        return Ok(hotels);
    }

    /// <summary>
    /// Adds a new hotel to the system (manager or above)
    /// </summary>
    /// <response code="201">Hotel created successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to add hotels</response>
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddHotel(AddHotelCommand cmd)
    {
        await mediator.Send(cmd);
        return Created();
    }

    /// <summary>
    /// Deletes a hotel by its ID  (manager or above)
    /// </summary>
    /// <response code="204">Hotel deleted successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to delete hotels</response>
    /// <response code="404">Hotel not found</response>
    [HttpDelete("{hotelId}")]
    [Authorize(Roles = "Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveHotel(int hotelId, CancellationToken ct)
    {
        await mediator.Send(new DeleteHotelCommand { HotelId = hotelId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns a specific hotel by its ID
    /// </summary>
    /// <response code="200">Returns the requested hotel</response>
    /// <response code="401">User is not authenticated (when authorization is required)</response>
    /// <response code="404">Hotel not found</response>
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
    /// Updates an existing hotel's information (manager or above)
    /// </summary>
    /// <response code="204">Hotel updated successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to update hotels</response>
    /// <response code="404">Hotel not found</response>
    [HttpPut]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotel([FromBody] UpdateHotelCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Updates the images for a specific hotel (manager or above)
    /// </summary>
    /// <response code="204">Hotel images updated successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to update hotel images</response>
    /// <response code="404">Hotel not found</response>
    [HttpPut("images")]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotelImages([FromForm] UpdateHotelImagesCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns weather information for a specific hotel location
    /// </summary>
    /// <response code="200">Returns weather data for the hotel location</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="404">Hotel not found</response>
    [HttpGet("weather/{hotelId}")]
    [ProducesResponseType(typeof(WeatherResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWeatherByHotelId(int hotelId, CancellationToken ct)
    {
        var response = await mediator.Send(new GetWeatherByHotelIdQuery { HotelId = hotelId }, ct);
        return Ok(response);
    }
}