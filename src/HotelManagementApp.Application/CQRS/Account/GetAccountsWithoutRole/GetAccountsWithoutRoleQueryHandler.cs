using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.AccountRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountsWithoutRole;

public class GetAccountsWithoutRoleQueryHandler(
    IUserManager userManager, 
    IFileService fileService,
    IProfilePictureRepository profilePictureRepository) : IRequestHandler<GetAccountsWithoutRoleQuery, ICollection<AccountResponse>>
{
    public async Task<ICollection<AccountResponse>> Handle(GetAccountsWithoutRoleQuery request, CancellationToken cancellationToken)
    {
        var users = await userManager.GetUsersWithoutRole();
        var response = new List<AccountResponse>();
        foreach (var user in users)
        {
            var profilePicture = await profilePictureRepository.GetProfilePicture(user.Id)
                ?? throw new ProfilePictureNotFoundException($"Profile picture of user with id {user.Id} not found");
            response.Add(new AccountResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = user.Roles,
                ProfilePicture = fileService.GetFileUrl("images", profilePicture.FileName)
            });
        }
        return response;
    }
}
