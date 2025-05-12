using HotelManagementApp.Application.CQRS.HotelServiceOps.Add;
using HotelManagementApp.Application.CQRS.HotelServiceOps.Delete;
using HotelManagementApp.Application.CQRS.HotelServiceOps.GetByHotel;
using HotelManagementApp.Application.CQRS.HotelServiceOps.GetById;
using HotelManagementApp.Application.CQRS.HotelServiceOps.Update;
using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[ApiController]
[Route("api/hotel-services")]
[Authorize]
public class HotelServicesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Adds hotel service (staff and above)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddHotelService([FromBody] AddHotelServiceCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return Created();
    }

    /// <summary>
    /// Updates hotel service (staff and above)
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateHotelService([FromBody] UpdateHotelServiceCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Delete hotel service (staff and above)
    /// </summary>
    [HttpDelete("{hotelServiceId}")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteHotelService(int hotelServiceId, CancellationToken ct)
    {
        await mediator.Send(new DeleteHotelServiceCommand { HotelServiceId = hotelServiceId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Get hotel services by hotel id
    /// </summary>
    [HttpGet("get-by-hotel/{hotelId}")]
    [ProducesResponseType(typeof(ICollection<HotelServiceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHotelServices(int hotelId, CancellationToken ct)
    {
        var hotelServices = await mediator.Send(new GetHotelServicesByHotelIdQuery { HotelId = hotelId }, ct);
        return Ok(hotelServices);
    }

    [HttpGet("{serviceId}")]
    public async Task<IActionResult> GetHotelServiceById(int serviceId, CancellationToken ct)
    {
        var response = await mediator.Send(new GetHotelServiceByIdQuery {ServiceId = serviceId}, ct);
        return Ok(response);
    }
    
    
}
