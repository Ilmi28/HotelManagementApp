using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountsWithoutRole;

public class GetAccountsWithoutRoleQueryHandler : IRequestHandler<GetAccountsWithoutRoleQuery>
{
    public Task Handle(GetAccountsWithoutRoleQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
