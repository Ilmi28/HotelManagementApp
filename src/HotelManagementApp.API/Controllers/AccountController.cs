using HotelManagementApp.Application.CQRS.Account.Create;
using HotelManagementApp.Application.CQRS.Account.Delete;
using HotelManagementApp.Application.CQRS.Account.GetAccountById;
using HotelManagementApp.Application.CQRS.Account.GetAccountsInRole;
using HotelManagementApp.Application.CQRS.Account.Update;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/account")]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand cmd)
        {
            await mediator.Send(cmd);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountCommand cmd)
        {
            await mediator.Send(cmd);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountCommand cmd)
        {
            await mediator.Send(cmd);
            return NoContent();
        }

        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetAccount(string id)
        {
            var account = await mediator.Send(new GetAccountQuery { UserId = id });
            return Ok(account);
        }

        [HttpGet("by-role/{role}")]
        public async Task<IActionResult> GetAccountsWithRole(string role)
        {
            var account = await mediator.Send(new GetAccountsInRoleQuery { RoleName = role });
            return Ok(account);
        }
    }
}
