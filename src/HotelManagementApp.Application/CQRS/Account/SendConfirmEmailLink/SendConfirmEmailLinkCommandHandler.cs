using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.SendConfirmEmailLink;

public class SendConfirmEmailLinkCommandHandler(IEmailService emailService) : IRequestHandler<SendConfirmEmailLinkCommand>
{
    public async Task Handle(SendConfirmEmailLinkCommand request, CancellationToken cancellationToken)
    {
        await emailService.SendEmailAsync("ilmialiev28@gmail.com", "Test", "Test", cancellationToken);
    }
}
