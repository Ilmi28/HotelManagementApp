using HotelManagementApp.Core.Responses.AccountResponses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountById
{
    public class GetAccountQuery : IRequest<AccountResponse>
    {
        public required string UserId { get; set; }
    }
}
