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
    /// Registers a new user and returns JWT token with refresh token
    /// </summary>
    /// <response code="200">User registered successfully, returns authentication tokens</response>
    /// <response code="400">Invalid registration data provided</response>
    /// <response code="409">User with provided email already exists</response>
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
    /// Authenticates user and returns JWT token with refresh token
    /// </summary>
    /// <response code="200">User authenticated successfully, returns authentication tokens</response>
    /// <response code="400">Invalid login data provided</response>
    /// <response code="401">Invalid credentials</response>
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
    /// Refreshes expired JWT token using refresh token
    /// </summary>
    /// <response code="200">Token refreshed successfully, returns new JWT token</response>
    /// <response code="400">Invalid refresh token provided</response>
    /// <response code="401">Refresh token expired or invalid</response>
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
