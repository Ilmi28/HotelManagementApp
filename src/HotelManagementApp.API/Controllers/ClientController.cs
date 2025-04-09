using HotelManagementApp.Application.CQRS.Client.AddToBlacklist;
using HotelManagementApp.Application.CQRS.Client.RemoveFromBlacklist;
using HotelManagementApp.Application.CQRS.Client.RemoveFromVIP;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/clients")]
public class ClientController(IMediator mediator) : ControllerBase
{
    [HttpPost("/blacklist/{userId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddToBlacklist(string userId)
    {
        await mediator.Send(new AddToBlacklistCommand { UserId = userId });
        return Created();
    }

    [HttpPost("blacklist/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromBlacklist(string userId)
    {
        await mediator.Send(new RemoveFromBlacklistCommand { UserId = userId });
        return NoContent();
    }

    [HttpPost("vip/{userId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddToVIP(string userId)
    {
        await mediator.Send(new AddToBlacklistCommand { UserId = userId });
        return Created();
    }

    [HttpDelete("vip/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromVIP(string userId)
    {
        await mediator.Send(new RemoveFromVIPCommand { UserId = userId });
        return NoContent();
    }
}
