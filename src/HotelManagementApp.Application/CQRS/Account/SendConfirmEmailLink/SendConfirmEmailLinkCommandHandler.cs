using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.TokenModels;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Application.CQRS.Account.SendConfirmEmailLink;

public class SendConfirmEmailLinkCommandHandler(
    IEmailService emailService,
    ITokenService tokenService,
    IUserManager userManager,
    IConfirmEmailTokensRepository tokenRepository,
    IConfiguration config) 
    : IRequestHandler<SendConfirmEmailLinkCommand>
{
    public async Task Handle(SendConfirmEmailLinkCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UnauthorizedAccessException();
        var emailConfirmationToken = tokenService.Generate512Token();
        var tokenHash = tokenService.GetTokenHash(emailConfirmationToken)
            ?? throw new InvalidOperationException("Token hash is null");
        var token = new ConfirmEmailToken
        {
            UserId = user.Id,
            ConfirmEmailTokenHash = tokenHash,
            ExpirationDate = DateTime.Now.AddHours(1)
        };
        var previousToken = await tokenRepository.GetTokenByUser(user.Id, cancellationToken);
        if (previousToken != null)
            await tokenRepository.DeleteToken(previousToken, cancellationToken);
        await tokenRepository.AddToken(token, cancellationToken);
        var htmlTemplatePath = "../HotelManagementApp.Application/HtmlTemplates/ConfirmEmail.html";
        var htmlTemplate = await File.ReadAllTextAsync(htmlTemplatePath, cancellationToken);
        var confirmEmailLink = $"{config["BaseUrl"]}/api/account/confirm-email?token={Uri.EscapeDataString(emailConfirmationToken)}&userId={user.Id}";
        htmlTemplate = htmlTemplate.Replace("{{ConfirmationLink}}", confirmEmailLink);
        await emailService.SendEmailAsync(user.Email, "Confirm email", htmlTemplate, cancellationToken);
    }
}
