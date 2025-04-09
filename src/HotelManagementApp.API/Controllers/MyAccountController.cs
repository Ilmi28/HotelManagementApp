using HotelManagementApp.API.Requests.MyAccount;
using HotelManagementApp.Application.CQRS.Account.ChangePassword;
using HotelManagementApp.Application.CQRS.Account.Delete;
using HotelManagementApp.Application.CQRS.Account.GetAccountById;
using HotelManagementApp.Application.CQRS.Account.Update;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManagementApp.API.Controllers;

[ApiController]
[Route("api/my-account")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MyAccountController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyAccount()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var account = await mediator.Send(new GetAccountQuery { UserId = userId });
        return Ok(account);
    }

    [HttpPatch("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var cmd = new ChangePasswordCommand
        {
            UserId = userId,
            OldPassword = request.OldPassword,
            NewPassword = request.NewPassword
        };
        await mediator.Send(cmd);
        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateAccount([FromBody] MyAccountUpdateRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var cmd = new UpdateAccountCommand
        {
            UserId = userId,
            UserName = request.UserName,
            Email = request.Email
        };
        await mediator.Send(cmd);
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAccount([FromBody] MyAccountDeleteRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var cmd = new DeleteAccountCommand
        {
            UserId = userId,
            Password = request.Password
        };
        await mediator.Send(cmd);
        return NoContent();
    }
}
