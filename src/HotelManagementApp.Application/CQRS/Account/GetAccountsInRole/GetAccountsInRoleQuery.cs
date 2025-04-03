using HotelManagementApp.Core.Responses.AccountResponses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountsInRole
{
    public class GetAccountsInRoleQuery : IRequest<ICollection<AccountResponse>>
    {
        public required string RoleName { get; set; }
    }
}
