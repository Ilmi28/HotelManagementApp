﻿using HotelManagementApp.Application.Responses.AuthResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories.AccountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.TokenRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.AccountModels;
using HotelManagementApp.Core.Models.TokenModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Auth.RegisterUser;

public class RegisterUserCommandHandler(IUserManager userManager,
                                    ITokenService tokenService,
                                    IRefreshTokenRepository tokenRepository,
                                    IProfilePictureRepository profilePictureRepository,
                                    IAccountDbLogger logger) : IRequestHandler<RegisterUserCommand, LoginRegisterResponse>
{
    private async Task CreateRefreshTokenInDb(string hashRefreshToken, UserDto user, CancellationToken ct)
    {
        var token = new RefreshToken
        {
            UserId = user.Id,
            RefreshTokenHash = hashRefreshToken,
            ExpirationDate = DateTime.Now.AddDays(tokenService.GetRefreshTokenExpirationDays())
        };
        await tokenRepository.AddToken(token, ct);
    }

    public async Task<LoginRegisterResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var userByEmail = await userManager.FindByEmailAsync(request.Email);
        var userByName = await userManager.FindByNameAsync(request.UserName);
        if (userByEmail != null)
            throw new UserExistsException("User with this email already exists.");
        if (userByName != null)
            throw new UserExistsException("User with this username already exists.");
        var user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Email = request.Email,
            Roles = ["Guest"]
        };
        await userManager.CreateAsync(user, request.Password);
        var profilePicture = new ProfilePicture
        {
            FileName = "defaultprofile.jpg",
            UserId = user.Id
        };
        await profilePictureRepository.AddProfilePicture(profilePicture, cancellationToken);
        string identityToken = tokenService.GenerateIdentityToken(user);
        string refreshToken = tokenService.Generate512Token();
        string hashRefreshToken = tokenService.GetTokenHash(refreshToken)
            ?? throw new Exception("Refresh token creation failed");
        await CreateRefreshTokenInDb(hashRefreshToken, user, cancellationToken);
        await logger.Log(AccountOperationEnum.Register, user);
        return new LoginRegisterResponse { IdentityToken = identityToken, RefreshToken = refreshToken };
    }
}
