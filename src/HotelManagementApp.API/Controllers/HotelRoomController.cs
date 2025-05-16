using HotelManagementApp.Application.CQRS.HotelRoomOps.Add;
using HotelManagementApp.Application.CQRS.HotelRoomOps.GetAll;
using HotelManagementApp.Application.CQRS.HotelRoomOps.GetById;
using HotelManagementApp.Application.CQRS.HotelRoomOps.GetRoomTypes;
using HotelManagementApp.Application.CQRS.HotelRoomOps.Remove;
using HotelManagementApp.Application.CQRS.HotelRoomOps.Update;
using HotelManagementApp.Application.CQRS.HotelRoomOps.UpdateRoomImages;
using HotelManagementApp.Application.Responses.RoomResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/hotel-room")]
[Authorize]
[ApiController]
public class HotelRoomController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Adds a new room to the hotel system (manager or above)
    /// </summary>
    /// <response code="201">Room created successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to add rooms</response>
    /// <response code="404">Hotel not found</response>
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return Created();
    }

    /// <summary>
    /// Returns all available room types in the system
    /// </summary>
    /// <response code="200">Returns list of room types</response>
    /// <response code="401">User is not authenticated</response>
    [HttpGet("get-room-types")]
    [ProducesResponseType(typeof(ICollection<RoomTypeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRoomTypes(CancellationToken ct)
    {
        var roomTypes = await mediator.Send(new GetRoomTypesQuery(), ct);
        return Ok(roomTypes);
    }

    /// <summary>
    /// Updates an existing room's information (manager or above)
    /// </summary>
    /// <response code="204">Room updated successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to update rooms</response>
    /// <response code="404">Room not found</response>
    [HttpPut]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a room by its ID (manager or above)
    /// </summary>
    /// <response code="204">Room deleted successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to delete rooms</response>
    /// <response code="404">Room not found</response>
    [HttpDelete("{roomId}")]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveRoom(int roomId, CancellationToken ct)
    {
        await mediator.Send(new RemoveRoomCommand { RoomId = roomId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns a specific room by its ID 
    /// </summary>
    /// <response code="200">Returns the requested room</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="404">Room not found</response>
    [HttpGet("{roomId}")]
    [ProducesResponseType(typeof(RoomResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomById(int roomId, CancellationToken ct)
    {
        var room = await mediator.Send(new GetRoomByIdQuery { RoomId = roomId }, ct);
        return Ok(room);
    }

    /// <summary>
    /// Returns all rooms in the system
    /// </summary>
    /// <response code="200">Returns list of all rooms</response>
    /// <response code="401">User is not authenticated</response>
    [HttpGet("get-all")]
    [ProducesResponseType(typeof(ICollection<RoomResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllRooms(CancellationToken ct)
    {
        var rooms = await mediator.Send(new GetAllRoomsQuery(), ct);
        return Ok(rooms);
    }

    /// <summary>
    /// Updates the images for a specific room (manager or above)
    /// </summary>
    /// <response code="204">Room images updated successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to update room images</response>
    /// <response code="404">Room not found</response>
    [HttpPut("images")]
    [Authorize(Roles = "Admin, Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoomImages([FromForm] UpdateRoomImagesCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }
}