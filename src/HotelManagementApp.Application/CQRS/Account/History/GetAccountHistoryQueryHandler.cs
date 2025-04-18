using HotelManagementApp.Application.Policies.AccountOwnerPolicy;
using HotelManagementApp.Application.Policies.RoleHierarchyPolicy;
using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.Application.CQRS.Account.History;

public class GetAccountHistoryQueryHandler(
    IUserManager userManager,
    IDbLogger<UserDto, AccountOperationEnum, UserLog> logger,
    IAuthenticationService authenticationService,
    IAuthorizationService authorizationService)
    : IRequestHandler<GetAccountHistoryQuery, ICollection<AccountLogResponse>>
{
    public async Task<ICollection<AccountLogResponse>> Handle(GetAccountHistoryQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UnauthorizedAccessException();
        var loggedInUser = authenticationService.GetLoggedInUser()
            ?? throw new UnauthorizedAccessException();
        var ownerPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new AccountOwnerRequirement());
        var hierarchyPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new RoleHierarchyRequirement());
        if (ownerPolicy.Succeeded || hierarchyPolicy.Succeeded)
        {
            var logs = await logger.GetLogs(user);
            return logs.Select(log => new AccountLogResponse
            {
                Operation = log.Operation.ToString(),
                OperationDate = log.Date,
            }).ToList();
        }
        else
            throw new PolicyForbiddenException("You do not have permission to access this resource");
    }
}
