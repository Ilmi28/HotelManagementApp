using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Responses.AccountResponses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountsInRole
{
    public class GetAccountsInRoleQueryHandler : IRequestHandler<GetAccountsInRoleQuery, ICollection<AccountResponse>>
    {
        private readonly IUserManager _userManager;
        public GetAccountsInRoleQueryHandler(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<ICollection<AccountResponse>> Handle(GetAccountsInRoleQuery request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {
                var users = await _userManager.GetUsersInRoleAsync(request.RoleName);
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
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured", ex);
            }
        }
    }
}
