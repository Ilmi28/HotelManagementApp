using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountsWithoutRole;

public class GetAccountsWithoutRoleQuery : IRequest<ICollection<AccountResponse>>
{

}
