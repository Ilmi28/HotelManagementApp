using HotelManagementApp.Application.Responses.AuthResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Auth.LoginUser
{
    public class LoginUserCommand : IRequest<LoginRegisterResponse>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
