using HotelManagementApp.Core.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Auth.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>   
    {
        public required string RefreshToken { get; set; }
    }
}
