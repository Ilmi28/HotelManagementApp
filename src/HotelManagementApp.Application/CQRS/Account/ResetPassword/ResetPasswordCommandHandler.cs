using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.TokenRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.ResetPassword;

public class ResetPasswordCommandHandler(
    IUserManager userManager,
    IResetPasswordTokenRepository tokenRepository,
    ITokenService tokenService) : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UnauthorizedAccessException();
        var hashToken = tokenService.GetTokenHash(Uri.UnescapeDataString(request.ResetPasswordToken))
            ?? throw new UnauthorizedAccessException();
        var token = await tokenRepository.GetToken(hashToken, cancellationToken)
            ?? throw new UnauthorizedAccessException();
        if (token.UserId != user.Id)
            throw new UnauthorizedAccessException();
        if (token.ExpirationDate < DateTime.UtcNow)
            throw new UnauthorizedAccessException();
        await tokenRepository.DeleteToken(token, cancellationToken);
        var result = await userManager.ResetPasswordAsync(user, request.NewPassword);
        if (!result)
            throw new Exception("Password reset failed.");
    }
}
