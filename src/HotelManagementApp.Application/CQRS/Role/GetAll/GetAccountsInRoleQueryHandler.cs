using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Application.CQRS.Role.GetAll;

public class GetAccountsInRoleQueryHandler(
    IUserRolesManager userRolesManager,
    IProfilePictureRepository profilePictureRepository,
    IConfiguration config) : IRequestHandler<GetAccountsInRoleQuery, ICollection<AccountResponse>>
{
    public async Task<ICollection<AccountResponse>> Handle(GetAccountsInRoleQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var users = await userRolesManager.GetUsersInRoleAsync(request.RoleName.Normalize());
        var accounts = new List<AccountResponse>();
        foreach (var user in users)
        {
            var profilePicture = await profilePictureRepository.GetProfilePicture(user.Id, cancellationToken)
                ?? throw new ProfilePictureNotFoundException($"Profile picture of user with id {user.Id} not found");
            var account = new AccountResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = user.Roles,
                ProfilePicture = $"{config.GetValue<string>("ImageUrl")}/{profilePicture.FileName}"
            };
            accounts.Add(account);
        }
        return accounts;
    }
}
