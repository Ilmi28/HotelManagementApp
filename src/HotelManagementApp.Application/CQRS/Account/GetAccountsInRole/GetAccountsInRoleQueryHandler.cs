using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Interfaces.Identity;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountsInRole
{
    public class GetAccountsInRoleQueryHandler(IUserManager userManager) : IRequestHandler<GetAccountsInRoleQuery, ICollection<AccountResponse>>
    {
        public async Task<ICollection<AccountResponse>> Handle(GetAccountsInRoleQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var users = await userManager.GetUsersInRoleAsync(request.RoleName.Normalize());
            var accounts = new List<AccountResponse>();
            foreach (var user in users)
            {
                var account = new AccountResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = user.Roles
                };
                accounts.Add(account);
            }
            return accounts;
        }
    }
}
