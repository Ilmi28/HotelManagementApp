using HotelManagementApp.API.Requests;
using HotelManagementApp.Application.CQRS.Account.ChangePassword;
using HotelManagementApp.Application.CQRS.Account.ConfirmEmail;
using HotelManagementApp.Application.CQRS.Account.Create;
using HotelManagementApp.Application.CQRS.Account.Delete;
using HotelManagementApp.Application.CQRS.Account.DeleteWithoutPassword;
using HotelManagementApp.Application.CQRS.Account.GetAccountById;
using HotelManagementApp.Application.CQRS.Account.GetAccountsWithoutRole;
using HotelManagementApp.Application.CQRS.Account.History;
using HotelManagementApp.Application.CQRS.Account.ResetPassword;
using HotelManagementApp.Application.CQRS.Account.SendConfirmEmailLink;
using HotelManagementApp.Application.CQRS.Account.SendPasswordResetLink;
using HotelManagementApp.Application.CQRS.Account.Update;
using HotelManagementApp.Application.CQRS.Account.UpdateProfilePicture;
using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManagementApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/account")]
public class AccountController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new account without any roles (staff or above)
    /// </summary>
    /// <response code="201">Account created successfully</response>
    /// <response code="403">User is not authorized to create accounts</response>
    [HttpPost("create")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request, CancellationToken ct)
    {
        var cmd = new CreateAccountCommand
        {
            UserName = request.UserName,
            Email = request.Email,
            Password = request.Password
        };
        await mediator.Send(cmd, ct);
        return Created();
    }

    /// <summary>
    /// Returns all accounts without any roles (staff or above)
    /// </summary>
    /// <response code="200">Returns list of accounts without roles</response>
    /// <response code="403">User is not authorized to view accounts</response>
    [Authorize(Roles = "Admin, Manager, Staff")]
    [HttpGet("without-role")]
    [ProducesResponseType(typeof(ICollection<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAccountsWithoutRole(CancellationToken ct)
    {
        var accounts = await mediator.Send(new GetAccountsWithoutRoleQuery(), ct);
        return Ok(accounts);
    }

    /// <summary>
    /// Updates base account information (owner or above)
    /// </summary>
    /// <response code="204">Account updated successfully</response>
    /// <response code="403">User is not authorized to update this account</response>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountCommand cmd, CancellationToken ct, IAuthorizationService authService)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }


    /// <summary>
    /// Deletes an account (owner or above)
    /// </summary>
    /// <response code="204">Account deleted successfully</response>
    /// <response code="403">User is not authorized to delete this account</response>
    [HttpPost("delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountCommand cmd, CancellationToken ct, IAuthorizationService authService)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns account information by ID (owner or above)
    /// </summary>
    /// <response code="200">Returns the requested account</response>
    /// <response code="403">User is not authorized to view this account</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAccount(string id, CancellationToken ct, IAuthorizationService authService)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, id, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, id, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();
        var account = await mediator.Send(new GetAccountQuery { UserId = id }, ct);
        return Ok(account);
    }


    /// <summary>
    /// Returns current session from JWT token
    /// </summary>
    /// <response code="200">Returns the current session</response>
    /// <response code="401">User is not authenticated</response>
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
    /// Deletes an account without password verification
    /// </summary>
    /// <response code="204">Account deleted successfully</response>
    /// <response code="403">User is not authorized to delete this account</response>
    [HttpDelete("delete-without-password/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAccountWithoutPassword(string userId, CancellationToken ct, IAuthorizationService authService)
    {
        var hierarchyPolicy = await authService.AuthorizeAsync(User, userId, "RoleHierarchy");
        if (!hierarchyPolicy.Succeeded)
            return Forbid();
        await mediator.Send(new DeleteWithoutPasswordCommand { UserId = userId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Changes account password after verifying current password
    /// </summary>
    /// <response code="204">Password changed successfully</response>
    /// <response code="403">User is not authorized to change this password</response>
    [HttpPatch("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand cmd, CancellationToken ct, IAuthorizationService authService)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "AccountOwner");
        if (!ownerPolicy.Succeeded)
            return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }


    /// <summary>
    /// Returns account history
    /// </summary>
    /// <response code="200">Returns the account history</response>
    /// <response code="403">User is not authorized to view this history</response>
    [HttpGet("{userId}/history")]
    [ProducesResponseType(typeof(ICollection<AccountLogResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetHistory(string userId, CancellationToken ct, IAuthorizationService authService)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, userId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, userId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();
        var history = await mediator.Send(new GetAccountHistoryQuery { UserId = userId }, ct);
        return Ok(history);
    }

    /// <summary>
    /// Uploads a profile picture for a user and returns created file name
    /// </summary>
    /// <response code="200">Returns the created file name</response>
    /// <response code="403">User is not authorized to upload this picture</response>
    [HttpPut("profile-picture")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UploadProfilePicture(UpdateProfilePictureCommand cmd, CancellationToken ct, IAuthorizationService authService)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();
        var fileName = await mediator.Send(cmd, ct);
        return Ok(fileName);
    }

    /// <summary>
    /// Sends confirmation email to user's email address
    /// </summary>
    /// <response code="204">Email sent successfully</response>
    /// <response code="403">User is not authorized to send confirmation email</response>
    [HttpPost("send-confirmation-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SendConfirmationEmail(SendConfirmEmailLinkCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var ownerPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "AccountOwner");
        var hierarchyPolicy = await authService.AuthorizeAsync(User, cmd.UserId, "RoleHierarchy");
        if (!ownerPolicy.Succeeded && !hierarchyPolicy.Succeeded)
            return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Confirms user's email address
    /// </summary>
    /// <response code="302">Email confirmed successfully, redirecting to frontend</response>
    /// <response code="400">Invalid confirmation token</response>
    [AllowAnonymous]
    [HttpPost("confirm-email")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand cmd, CancellationToken ct, IConfiguration config)
    {
        await mediator.Send(cmd, ct);
        return Redirect(config["FrontendUrl"]!);
    }

    /// <summary>
    /// Sends password reset link to user's email address
    /// </summary>
    /// <response code="204">Reset link sent successfully</response>
    /// <response code="400">Invalid email address</response>
    [AllowAnonymous]
    [HttpPost]
    [Route("send-password-reset-link")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendPasswordResetLink(SendPasswordResetLinkCommand cmd, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Resets user's password using reset token
    /// </summary>
    /// <response code="204">Password reset successfully</response>
    /// <response code="400">Invalid reset token</response>
    [AllowAnonymous]
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand cmd, IConfiguration config, CancellationToken ct)
    {
        await mediator.Send(cmd, ct);
        return NoContent();

    }
}
