using HotelManagementApp.Application.CQRS.HotelRoomOps.Add;
using HotelManagementApp.Application.CQRS.HotelRoomOps.GetAll;
using HotelManagementApp.Application.CQRS.HotelRoomOps.GetById;
using HotelManagementApp.Application.CQRS.HotelRoomOps.GetRoomTypes;
using HotelManagementApp.Application.CQRS.HotelRoomOps.Remove;
using HotelManagementApp.Application.CQRS.HotelRoomOps.Update;
using HotelManagementApp.Application.CQRS.HotelRoomOps.UpdateRoomImages;
using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Application.Responses.RoomResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/hotel-room")]
[Authorize]
[ApiController]
public class HotelRoomController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Add room (staff and above)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return Created();
    }

    /// <summary>
    /// Get all room types
    /// </summary>
    [HttpGet("get-room-types")]
    [ProducesResponseType(typeof(ICollection<RoomTypeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRoomTypes(CancellationToken ct)
    {
        var roomTypes = await mediator.Send(new GetRoomTypesQuery(), ct);
        return Ok(roomTypes);
    }

    /// <summary>
    /// Update room (staff and above)
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Delete room by id (staff and above)
    /// </summary>
    [HttpDelete("{roomId}")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveRoom(int roomId, CancellationToken ct)
    {
        await mediator.Send(new RemoveRoomCommand { RoomId = roomId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Get room by id
    /// </summary>
    [HttpGet("{roomId}")]
    [ProducesResponseType(typeof(HotelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomById(int roomId, CancellationToken ct)
    {
        var room = await mediator.Send(new GetRoomByIdQuery { RoomId = roomId }, ct);
        return Ok(room);
    }

    /// <summary>
    /// Get all rooms
    /// </summary>
    [HttpGet("get-all")]
    [ProducesResponseType(typeof(ICollection<RoomResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllRooms(CancellationToken ct)
    {
        var rooms = await mediator.Send(new GetAllRoomsQuery(), ct);
        return Ok(rooms);
    }

    /// <summary>
    /// Updates room images (staff and above)
    /// </summary>
    [HttpPut("images")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateRoomImages([FromForm] UpdateRoomImagesCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }
}
