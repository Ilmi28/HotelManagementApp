using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Role.GetAll;

public class GetAccountsInRoleQuery : IRequest<ICollection<AccountResponse>>
{
    public required string RoleName { get; set; }
}
