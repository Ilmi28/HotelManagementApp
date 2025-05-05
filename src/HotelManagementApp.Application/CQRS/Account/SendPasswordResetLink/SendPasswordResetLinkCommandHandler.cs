using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.TokenRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.TokenModels;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Application.CQRS.Account.SendPasswordResetLink;

public class SendPasswordResetLinkCommandHandler(
    IEmailService emailService,
    ITokenService tokenService,
    IUserManager userManager,
    IResetPasswordTokenRepository tokenRepository,
    IConfiguration config)
    : IRequestHandler<SendPasswordResetLinkCommand>
{
    public async Task Handle(SendPasswordResetLinkCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException();
        var resetPasswordToken = tokenService.Generate512Token();
        var tokenHash = tokenService.GetTokenHash(resetPasswordToken)
            ?? throw new InvalidOperationException("Token hash is null");
        var token = new ResetPasswordToken
        {
            UserId = user.Id,
            ResetPasswordTokenHash = tokenHash,
            ExpirationDate = DateTime.Now.AddMinutes(15)
        };
        var previousToken = await tokenRepository.GetTokenByUser(user.Id, cancellationToken);
        if (previousToken != null)
            await tokenRepository.DeleteToken(previousToken, cancellationToken);
        await tokenRepository.AddToken(token, cancellationToken);
        var htmlTemplatePath = "../HotelManagementApp.Application/HtmlTemplates/ResetPassword.html";
        var htmlTemplate = await File.ReadAllTextAsync(htmlTemplatePath, cancellationToken);
        var confirmEmailLink = $"{config["FrontendUrl"]}/account/reset-password?token={Uri.EscapeDataString(resetPasswordToken)}&userId={user.Id}";
        htmlTemplate = htmlTemplate.Replace("{{ResetPasswordLink}}", confirmEmailLink);
        await emailService.SendEmailAsync(user.Email, "Reset password", htmlTemplate, cancellationToken);
    }
}
