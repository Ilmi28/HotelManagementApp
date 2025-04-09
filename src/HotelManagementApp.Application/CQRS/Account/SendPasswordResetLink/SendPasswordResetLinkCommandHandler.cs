using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.SendPasswordResetLink;

public class SendPasswordResetLinkCommandHandler : IRequestHandler<SendPasswordResetLinkCommand>
{
    public Task Handle(SendPasswordResetLinkCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
