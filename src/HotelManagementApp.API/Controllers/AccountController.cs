using HotelManagementApp.Application.CQRS.Account.Create;
using HotelManagementApp.Application.CQRS.Account.Delete;
using HotelManagementApp.Application.CQRS.Account.GetAccountById;
using HotelManagementApp.Application.CQRS.Account.GetAccountsInRole;
using HotelManagementApp.Application.CQRS.Account.Update;
using HotelManagementApp.Application.CQRS.MyAccount.ChangePassword;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManagementApp.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand cmd)
        {
            await _mediator.Send(cmd);
            return Ok();
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountCommand cmd)
        {
            await _mediator.Send(cmd);
            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountCommand cmd)
        {
            await _mediator.Send(cmd);
            return Ok();
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetAccount(string id)
        {
            var account = await _mediator.Send(new GetAccountQuery { UserId = id });
            return Ok(account);
        }

        [HttpGet("my-account")]
        public async Task<IActionResult> GetMyAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var account = await _mediator.Send(new GetAccountQuery { UserId = userId });
            return Ok(account);
        }

        [HttpGet("get-with-role/{role}")]
        public async Task<IActionResult> GetAccountWithRole(string role)
        {
            var account = await _mediator.Send(new GetAccountsInRoleQuery { RoleName = role });
            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand cmd)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            cmd.UserId = userId;
            await _mediator.Send(cmd);
            return Ok();
        }
    }
}
