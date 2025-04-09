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
    public class ClientController(IMediator mediator) : ControllerBase
    {
        [HttpPost("api/clients/add-to-blacklist")]
        public async Task<IActionResult> AddToBlacklist([FromBody] AddToBlacklistCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpPost("api/clients/remove-from-blacklist")]
        public async Task<IActionResult> RemoveFromBlacklist([FromBody] RemoveFromBlacklistCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpPost("api/clients/add-to-vip")]
        public async Task<IActionResult> AddToVIP([FromBody] AddToVIPCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpPost("api/clients/remove-from-vip")]
        public async Task<IActionResult> RemoveFromVIP([FromBody] RemoveFromBlacklistCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }
    }
}
