using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountsInRole;

public class GetAccountsInRoleQuery : IRequest<ICollection<AccountResponse>>
{
    public required string RoleName { get; set; }
}
