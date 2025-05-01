using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.ConfirmEmail;

public class ConfirmEmailCommandHandler(
    IUserManager userManager, 
    IConfirmEmailTokensRepository tokenRepository,
    ITokenService tokenService) 
    : IRequestHandler<ConfirmEmailCommand>
{
    public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UnauthorizedAccessException();
        var hashToken = tokenService.GetTokenHash(request.Token)
            ?? throw new UnauthorizedAccessException();
        var token = await tokenRepository.GetToken(hashToken, cancellationToken)
            ?? throw new UnauthorizedAccessException();
        if (token.UserId != user.Id)
            throw new UnauthorizedAccessException();
        if (token.ExpirationDate < DateTime.UtcNow)
            throw new UnauthorizedAccessException();
        if (user.IsEmailConfirmed)
            throw new InvalidOperationException("Email is already confirmed");
        user.IsEmailConfirmed = true;
        await tokenRepository.DeleteToken(token, cancellationToken);
        await userManager.UpdateAsync(user);
    }
}
