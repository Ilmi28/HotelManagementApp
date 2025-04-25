using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountById;

public class GetAccountQueryHandler(
    IUserManager userManager,
    IProfilePictureRepository profilePictureRepository,
    IConfiguration config) : IRequestHandler<GetAccountQuery, AccountResponse>
{
    public async Task<AccountResponse> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId) 
            ?? throw new UserNotFoundException($"User with id {request.UserId} not found");
        var profilePicture = await profilePictureRepository.GetProfilePicture(user.Id, cancellationToken)
                ?? throw new ProfilePictureNotFoundException($"Profile picture of user with id {user.Id} not found");
        var response = new AccountResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = user.Roles,
            ProfilePicture = $"{config["ImageUrl"]}/{profilePicture.FileName}"
        };
        return response;
    }
}
