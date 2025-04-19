using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.GetAll;

public class GetVIPListQueryHandler(IVIPRepository vipRepository, IUserManager userManager) 
    : IRequestHandler<GetVIPListQuery, ICollection<AccountResponse>>
{
    public async Task<ICollection<AccountResponse>> Handle(GetVIPListQuery request, CancellationToken cancellationToken)
    {
        var vipList = await vipRepository.GetVIPUsers(cancellationToken);
        var accounts = new List<AccountResponse>();
        foreach (var vip in vipList)
        {
            var user = await userManager.FindByIdAsync(vip.UserId);
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
