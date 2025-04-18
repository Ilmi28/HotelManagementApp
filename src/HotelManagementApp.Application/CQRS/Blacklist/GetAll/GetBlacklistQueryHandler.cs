using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Blacklist.GetAll;

public class GetBlacklistQueryHandler(IBlacklistRepository blacklistRepository, IUserManager userManager) : IRequestHandler<GetBlacklistQuery, ICollection<UserDto>>
{
    public async Task<ICollection<UserDto>> Handle(GetBlacklistQuery request, CancellationToken cancellationToken)
    {
        var blacklist = await blacklistRepository.GetBlackList(cancellationToken);
        var users = new List<UserDto>();
        foreach (var blacklistUser in blacklist)
        {
            var user = await userManager.FindByIdAsync(blacklistUser.UserId);
            if (user != null)
                users.Add(user);
        }
        return users;
    }
}
