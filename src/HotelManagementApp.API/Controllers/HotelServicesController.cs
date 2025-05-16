using HotelManagementApp.Application.CQRS.HotelServiceOps.Add;
using HotelManagementApp.Application.CQRS.HotelServiceOps.Delete;
using HotelManagementApp.Application.CQRS.HotelServiceOps.GetByHotel;
using HotelManagementApp.Application.CQRS.HotelServiceOps.GetById;
using HotelManagementApp.Application.CQRS.HotelServiceOps.Update;
using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[ApiController]
[Route("api/hotel-services")]
[Authorize]
public class HotelServicesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Adds a new service to a hotel (manager or above)
    /// </summary>
    /// <response code="201">Service created successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to add hotel services</response>
    /// <response code="404">Hotel not found</response>
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddHotelService([FromBody] AddHotelServiceCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return Created();
    }

    /// <summary>
    /// Updates an existing hotel service (manager or above)
    /// </summary>
    /// <response code="204">Service updated successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to update hotel services</response>
    /// <response code="404">Service not found</response>
    [HttpPut]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotelService([FromBody] UpdateHotelServiceCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a hotel service by its ID (manager or above)
    /// </summary>
    /// <response code="204">Service deleted successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to delete hotel services</response>
    /// <response code="404">Service not found</response>
    [HttpDelete("{hotelServiceId}")]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHotelService(int hotelServiceId, CancellationToken ct)
    {
        await mediator.Send(new DeleteHotelServiceCommand { HotelServiceId = hotelServiceId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns all services available at a specific hotel
    /// </summary>
    /// <response code="200">Returns list of hotel services</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="404">Hotel not found</response>
    [HttpGet("get-by-hotel/{hotelId}")]
    [ProducesResponseType(typeof(ICollection<HotelServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelServices(int hotelId, CancellationToken ct)
    {
        var hotelServices = await mediator.Send(new GetHotelServicesByHotelIdQuery { HotelId = hotelId }, ct);
        return Ok(hotelServices);
    }

    /// <summary>
    /// Returns a specific hotel service by its ID
    /// </summary>
    /// <response code="200">Returns the requested hotel service</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="404">Service not found</response>
    [HttpGet("{serviceId}")]
    [ProducesResponseType(typeof(HotelServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelServiceById(int serviceId, CancellationToken ct)
    {
        var response = await mediator.Send(new GetHotelServiceByIdQuery {ServiceId = serviceId}, ct);
        return Ok(response);
    }
}