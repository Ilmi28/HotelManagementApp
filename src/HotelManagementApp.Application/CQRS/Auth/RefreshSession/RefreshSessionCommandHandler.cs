﻿using HotelManagementApp.Application.Responses.AuthResponses;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.TokenRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Auth.RefreshSession;

public class RefreshSessionCommandHandler(ITokenService tokenService,
                                    IRefreshTokenRepository tokenRepository,
                                    IUserManager userManager) : IRequestHandler<RefreshSessionCommand, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshSessionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        string hash = tokenService.GetTokenHash(request.RefreshToken)
                                                    ?? throw new UnauthorizedAccessException();
        var token = await tokenRepository.GetToken(hash, cancellationToken)
                                                ?? throw new UnauthorizedAccessException();
        if (token.ExpirationDate < DateTime.Now)
            throw new UnauthorizedAccessException();
        var user = await userManager.FindByIdAsync(token.UserId)
                                                ?? throw new UnauthorizedAccessException();
        var identityToken = tokenService.GenerateIdentityToken(user);
        return new RefreshTokenResponse { IdentityToken = identityToken };
    }
}
