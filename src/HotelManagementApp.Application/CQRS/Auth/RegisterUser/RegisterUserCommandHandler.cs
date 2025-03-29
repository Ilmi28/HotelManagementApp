using HotelManagementApp.Core.Exceptions.Database;
using HotelManagementApp.Core.Exceptions.UserExceptions;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Responses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Auth.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, LoginRegisterResponse>
    {
        private IAuthService _authService;

        public RegisterUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginRegisterResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {
                var roles = new List<string> { "User" };
                (string identityToken, string refreshToken) = await _authService.RegisterUser(request.UserName, request.Email, request.Password, roles);
                return new LoginRegisterResponse { IdentityToken = identityToken, RefreshToken = refreshToken };
            }
            catch { throw; }
        }
    }
}
