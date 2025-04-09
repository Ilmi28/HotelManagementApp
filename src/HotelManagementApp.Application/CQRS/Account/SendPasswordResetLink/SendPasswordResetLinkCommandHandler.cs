using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Account.SendPasswordResetLink
{
    public class SendPasswordResetLinkCommandHandler : IRequestHandler<SendPasswordResetLinkCommand>
    {
        public Task Handle(SendPasswordResetLinkCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
