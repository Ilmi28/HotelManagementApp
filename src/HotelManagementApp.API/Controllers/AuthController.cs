using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Application.CQRS.Auth.RefreshToken;
using HotelManagementApp.Application.CQRS.Auth.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HotelManagementApp.Application.Responses.AuthResponses;

namespace HotelManagementApp.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand cmd)
        {
            var response = await mediator.Send(cmd);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand cmd)
        {
            var response = await mediator.Send(cmd);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand cmd)
        {
            var response = await mediator.Send(cmd);
            return Ok(response);
        }
    }
}
