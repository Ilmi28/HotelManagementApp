using System.Security.Claims;
using HotelManagementApp.API.AppProblemDetails;
using HotelManagementApp.Core.Interfaces.Repositories.GuestRepositories;

namespace HotelManagementApp.API.AppMiddleware;

public class BlacklistMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IBlacklistRepository blacklistRepository)
    {
        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;;
        var ct = context.RequestAborted;
        if (!string.IsNullOrEmpty(userId) && await blacklistRepository.IsUserBlacklisted(userId, ct))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await next(context);
    }
}