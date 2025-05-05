using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.AccountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.GuestRepositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Application.CQRS.VIP.GetAll;

public class GetVIPListQueryHandler(IVIPRepository vipRepository, 
                    IUserManager userManager, 
                    IProfilePictureRepository profilePictureRepository,
                    IFileService fileService) 
                    : IRequestHandler<GetVIPListQuery, ICollection<AccountResponse>>
{
    public async Task<ICollection<AccountResponse>> Handle(GetVIPListQuery request, CancellationToken cancellationToken)
    {
        var vipList = await vipRepository.GetVIPUsers(cancellationToken);
        var accounts = new List<AccountResponse>();
        foreach (var vip in vipList)
        {
            var user = await userManager.FindByIdAsync(vip.UserId)
                ?? throw new UserNotFoundException($"User with id {vip.UserId} not found");
            var profilePicture = await profilePictureRepository.GetProfilePicture(vip.UserId, cancellationToken)
                ?? throw new ProfilePictureNotFoundException($"Profile picture of user with id {vip.UserId} not found");
            if (user != null)
            {
                var account = new AccountResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = user.Roles,
                    ProfilePicture = fileService.GetFileUrl("images",profilePicture.FileName),
                };
                accounts.Add(account);
            }
        }
        return accounts;
    }
}
