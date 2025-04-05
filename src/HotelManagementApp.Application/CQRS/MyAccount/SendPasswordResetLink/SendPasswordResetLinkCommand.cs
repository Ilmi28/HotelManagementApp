using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.MyAccount.SendPasswordResetLink
{
    public class SendPasswordResetLinkCommand : IRequest
    {
        public required string Email { get; set; }
    }
}
