using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.GetAll;

public class GetVIPListQueryHandler(IVIPRepository vipRepository, IUserManager userManager) 
    : IRequestHandler<GetVIPListQuery, ICollection<UserDto>>
{
    public async Task<ICollection<UserDto>> Handle(GetVIPListQuery request, CancellationToken cancellationToken)
    {
        var vipList = await vipRepository.GetVIPUsers();
        var users = new List<UserDto>();
        foreach (var vip in vipList)
        {
            var user = await userManager.FindByIdAsync(vip.UserId);
            if (user != null)
                users.Add(user);
        }
        return users;
    }
}
