using HotelManagementApp.API.Requests;
using HotelManagementApp.Application.CQRS.Account.ChangePassword;
using HotelManagementApp.Application.CQRS.Account.Delete;
using HotelManagementApp.Application.CQRS.Account.DeleteWithoutPassword;
using HotelManagementApp.Application.CQRS.Account.GetAccountById;
using HotelManagementApp.Application.CQRS.Account.History;
using HotelManagementApp.Application.CQRS.Account.Update;
using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManagementApp.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/account")]
public class AccountController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new account without any roles (for staff and above).
    /// </summary>
    [HttpPost("create")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return Created();
    }

    /// <summary>
    /// Updates base account information (higher in the hierarchy or owner).
    /// </summary>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }


    /// <summary>
    /// Deletes an account (higher in the hierarchy or owner)
    /// </summary>
    [HttpPost("delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns account information by ID (higher in the hierarchy or owner).
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAccount(string id, CancellationToken ct)
    {
        var account = await mediator.Send(new GetAccountQuery { UserId = id }, ct);
        return Ok(account);
    }


    /// <summary>
    /// Returns current session from JWT token.
    /// </summary>
    [HttpGet("session")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetSession(CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
        var account = await mediator.Send(new GetAccountQuery { UserId = userId }, ct);
        return Ok(account);
    }

    /// <summary>
    /// Deletes an account without password (higher in the hierarchy).
    /// </summary>
    [HttpDelete("delete-without-password/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAccountWithoutPassword(string userId, CancellationToken ct)
    {
        await mediator.Send(new DeleteWithoutPasswordCommand { UserId = userId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Changes password after providing previous one (owner).
    /// </summary>
    [HttpPatch("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }


    /// <summary>
    /// Returns account history (higher in the hierarchy or owner).
    /// </summary>
    [HttpGet("history/{userId}")]
    [ProducesResponseType(typeof(ICollection<AccountLogResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHistory(string userId, CancellationToken ct)
    {
        var history = await mediator.Send(new GetAccountHistoryQuery { UserId = userId }, ct);
        return Ok(history);
    }


}
