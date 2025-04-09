using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Client.AddToVIP
{
    public class AddToVIPCommand : IRequest
    {
        public required string UserId { get; set; }
    }
}
