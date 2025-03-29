using HotelManagementApp.Core.Exceptions.Database;
using HotelManagementApp.Core.Responses;
using MediatR;
using HotelManagementApp.Core.Interfaces.Services;

namespace HotelManagementApp.Application.CQRS.Auth.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginRegisterResponse>
    {
        private IAuthService _authService;
        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginRegisterResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {

                (string identityToken, string refreshToken) = await _authService.LoginUser(request.Email, request.Password);
                return new LoginRegisterResponse { IdentityToken = identityToken, RefreshToken = refreshToken };
            }
            catch { throw; }
        }
    }
}
