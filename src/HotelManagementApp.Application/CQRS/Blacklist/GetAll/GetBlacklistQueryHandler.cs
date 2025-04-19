using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Blacklist.GetAll;

public class GetBlacklistQueryHandler(IBlacklistRepository blacklistRepository, IUserManager userManager) 
    : IRequestHandler<GetBlacklistQuery, ICollection<AccountResponse>>
{
    public async Task<ICollection<AccountResponse>> Handle(GetBlacklistQuery request, CancellationToken cancellationToken)
    {
        var blacklist = await blacklistRepository.GetBlackList(cancellationToken);
        var accounts = new List<AccountResponse>();
        foreach (var blacklistUser in blacklist)
        {
            var user = await userManager.FindByIdAsync(blacklistUser.UserId);
            if (user != null)
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
        }
        return accounts;
    }
}
