using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Application.CQRS.Auth.RefreshToken;
using HotelManagementApp.Application.CQRS.Auth.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand cmd)
        {
            var response = await _mediator.Send(cmd);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand cmd)
        {
            var response = await _mediator.Send(cmd);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand cmd)
        {
            var response = await _mediator.Send(cmd);
            return Ok(response);
        }
    }
}
