using HotelManagementApp.Application.Policies.AccountOwnerPolicy;
using HotelManagementApp.Application.Policies.RoleHierarchyPolicy;
using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountById;

public class GetAccountQueryHandler(
    IUserManager userManager, 
    IAuthorizationService authorizationService,
    IAuthenticationService authenticationService,
    IProfilePictureRepository profilePictureRepository,
    IConfiguration config) : IRequestHandler<GetAccountQuery, AccountResponse>
{
    public async Task<AccountResponse> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId) 
            ?? throw new UserNotFoundException($"User with id {request.UserId} not found");
        var loggedInUser = authenticationService.GetLoggedInUser();
        if (loggedInUser == null)
            throw new UnauthorizedAccessException();
        var hierarchyPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new RoleHierarchyRequirement());
        var ownerPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new AccountOwnerRequirement());
        if (hierarchyPolicy.Succeeded || ownerPolicy.Succeeded)
        {
            var profilePicture = await profilePictureRepository.GetProfilePicture(user.Id, cancellationToken)
                ?? throw new ProfilePictureNotFoundException($"Profile picture of user with id {user.Id} not found");
            var response = new AccountResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = user.Roles,
                ProfilePicture = $"{config.GetValue<string>("ImageUrl")}/{profilePicture.FileName}"
            };
            return response;
        }
        else
            throw new PolicyForbiddenException("You do not have permission to access this resource");
    }
}
