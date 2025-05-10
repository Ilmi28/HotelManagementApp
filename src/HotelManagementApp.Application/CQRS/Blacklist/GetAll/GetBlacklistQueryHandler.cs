using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.AccountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.GuestRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Application.CQRS.Blacklist.GetAll;

public class GetBlacklistQueryHandler(
    IBlacklistRepository blacklistRepository, 
    IUserManager userManager,
    IProfilePictureRepository profilePictureRepository,
    IFileService fileService
    ) : IRequestHandler<GetBlacklistQuery, ICollection<AccountResponse>>
{
    public async Task<ICollection<AccountResponse>> Handle(GetBlacklistQuery request, CancellationToken cancellationToken)
    {
        var blacklist = await blacklistRepository.GetBlackList(cancellationToken);
        var accounts = new List<AccountResponse>();
        foreach (var blacklistUser in blacklist)
        {
            var user = await userManager.FindByIdAsync(blacklistUser.UserId)
                       ?? throw new UserNotFoundException($"User with id {blacklistUser.UserId} not found");
            var profilePicture = await profilePictureRepository.GetProfilePicture(blacklistUser.UserId, cancellationToken)
                                 ?? throw new ProfilePictureNotFoundException($"Profile picture of user with id {blacklistUser.UserId} not found");
            var account = new AccountResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = user.Roles,
                ProfilePicture = fileService.GetFileUrl("images",profilePicture.FileName),
                IsEmailConfirmed = user.IsEmailConfirmed
            };
            accounts.Add(account);
        }
        return accounts;
    }
}
