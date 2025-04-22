using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Application.CQRS.Auth.RefreshSession;
using HotelManagementApp.Application.CQRS.Auth.RegisterUser;
using HotelManagementApp.Application.Responses.AuthResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Returns JWT token and refresh token for the user if successful.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(LoginRegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand cmd, CancellationToken ct)
    {
        var response = await mediator.Send(cmd, ct);
        return Ok(response);
    }

    /// <summary>
    /// Returns JWT token and refresh token for the user if successful.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginRegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand cmd, CancellationToken ct)
    {
        var response = await mediator.Send(cmd, ct);
        return Ok(response);
    }

    /// <summary>
    /// Returns new JWT token.
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshSessionCommand cmd, CancellationToken ct)
    {
        var response = await mediator.Send(cmd, ct);
        return Ok(response);
    }
}
