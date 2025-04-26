using HotelManagementApp.Application.CQRS.Hotel.UpdateHotelImages;
using HotelManagementApp.Application.CQRS.Room.Add;
using HotelManagementApp.Application.CQRS.Room.GetAll;
using HotelManagementApp.Application.CQRS.Room.GetById;
using HotelManagementApp.Application.CQRS.Room.GetRoomTypes;
using HotelManagementApp.Application.CQRS.Room.Remove;
using HotelManagementApp.Application.CQRS.Room.Update;
using HotelManagementApp.Application.CQRS.Room.UpdateRoomImages;
using HotelManagementApp.Application.Responses.RoomResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/room")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    Roles = "Staff, Manager, Admin")]
[ApiController]
public class RoomController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Add room (staff and above)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return Created();
    }

    /// <summary>
    /// Get all room types (staff and above)
    /// </summary>
    [HttpGet("get-room-types")]
    public async Task<IActionResult> GetRoomTypes(CancellationToken ct)
    {
        var roomTypes = await mediator.Send(new GetRoomTypesQuery(), ct);
        return Ok(roomTypes);
    }

    /// <summary>
    /// Update room (staff and above)
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Delete room by id (staff and above)
    /// </summary>
    [HttpDelete("{roomId}")]
    public async Task<IActionResult> RemoveRoom(int roomId, CancellationToken ct)
    {
        await mediator.Send(new RemoveRoomCommand { RoomId = roomId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Get room by id (staff and above)
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet("{roomId}")]
    public async Task<IActionResult> GetRoomById(int roomId, CancellationToken ct)
    {
        var room = await mediator.Send(new GetRoomByIdQuery { RoomId = roomId }, ct);
        return Ok(room);
    }

    /// <summary>
    /// Get all rooms (staff and above)
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateRoomImages([FromForm] UpdateRoomImagesCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }
}
