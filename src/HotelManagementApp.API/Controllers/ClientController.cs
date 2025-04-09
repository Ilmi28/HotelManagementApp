using HotelManagementApp.Application.CQRS.Client.AddToBlacklist;
using HotelManagementApp.Application.CQRS.Client.AddToVIP;
using HotelManagementApp.Application.CQRS.Client.RemoveFromBlacklist;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/clients")]
    public class ClientController(IMediator mediator) : ControllerBase
    {
        [HttpPost("/blacklist/add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddToBlacklist([FromBody] AddToBlacklistCommand command)
        {
            await mediator.Send(command);
            return Created();
        }

        [HttpPost("blacklist/remove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromBlacklist([FromBody] RemoveFromBlacklistCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPost("vip/add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddToVIP([FromBody] AddToVIPCommand command)
        {
            await mediator.Send(command);
            return Created();
        }

        [HttpPost("vip/remove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromVIP([FromBody] RemoveFromBlacklistCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
    }
}
