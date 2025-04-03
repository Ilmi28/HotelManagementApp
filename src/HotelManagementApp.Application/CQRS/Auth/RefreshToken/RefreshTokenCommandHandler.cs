using HotelManagementApp.Core.Exceptions;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Responses.AuthResponses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private ITokenManager _tokenManager;
        private ITokenRepository _tokenRepository;
        private IUserManager _userManager;
        public RefreshTokenCommandHandler(ITokenManager tokenManager,
                                            ITokenRepository tokenRepository,
                                            IUserManager userManager)
        {
            _tokenManager = tokenManager;
            _tokenRepository = tokenRepository;
            _userManager = userManager;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {
                string hash = _tokenManager.GetHashRefreshToken(request.RefreshToken)
                                                        ?? throw new UnauthorizedAccessException();
                var token = await _tokenRepository.GetToken(hash)
                                                        ?? throw new UnauthorizedAccessException();
                var user = await _userManager.FindByIdAsync(token.UserId)
                                                        ?? throw new UnauthorizedAccessException();
                var identityToken = _tokenManager.GenerateIdentityToken(user);
                return new RefreshTokenResponse { IdentityToken = identityToken };
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex) { throw new Exception("Unexpected error occured", ex); }
        }
    }
}
