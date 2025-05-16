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
    /// <summary>
    /// Adds a new parking facility to a hotel (manager or above)
    /// </summary>
    /// <response code="201">Parking facility added successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to add parking facilities</response>
    /// <response code="404">Hotel not found</response>
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddHotelParking([FromBody] AddHotelParkingCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return Created();
    }

    /// <summary>
    /// Returns all parking facilities for a specific hotel
    /// </summary>
    /// <response code="200">Returns list of parking facilities for the hotel</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="404">Hotel not found</response>
    [HttpGet("get-by-hotel/{hotelId}")]
    [ProducesResponseType(typeof(ICollection<HotelParkingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelParkings(int hotelId, CancellationToken ct)
    {
        var hotelParkings = await mediator.Send(new GetHotelParkingsByHotelIdQuery { HotelId = hotelId }, ct);
        return Ok(hotelParkings);
    }

    /// <summary>
    /// Deletes a hotel parking facility by its ID (manager or above)
    /// </summary>
    /// <response code="204">Parking facility deleted successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to delete parking facilities</response>
    /// <response code="404">Parking facility not found</response>
    [HttpDelete("{hotelParkingId}")]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHotelParking(int hotelParkingId, CancellationToken ct)
    {
        await mediator.Send(new DeleteHotelParkingCommand { Id = hotelParkingId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Updates an existing hotel parking facility information (manager or above)
    /// </summary>
    /// <response code="204">Parking facility updated successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to update parking facilities</response>
    /// <response code="404">Parking facility not found</response>
    [HttpPut]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotelParking([FromBody] UpdateHotelParkingCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns a specific hotel parking facility by its ID 
    /// </summary>
    /// <response code="200">Returns the requested parking facility</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="404">Parking facility not found</response>
    [HttpGet("{parkingId}")]
    [ProducesResponseType(typeof(HotelParkingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelParkingById(int parkingId, CancellationToken ct)
    {
        var response = await mediator.Send(new GetHotelParkingByIdQuery { ParkingId = parkingId }, ct);
        return Ok(response);
    }
}