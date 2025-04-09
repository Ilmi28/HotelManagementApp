using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Interfaces.Identity;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountById;

public class GetAccountQueryHandler(IUserManager userManager) : IRequestHandler<GetAccountQuery, AccountResponse>
{
    public async Task<AccountResponse> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId) ?? throw new UnauthorizedAccessException();
        return new AccountResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = user.Roles
        };
    }
}
