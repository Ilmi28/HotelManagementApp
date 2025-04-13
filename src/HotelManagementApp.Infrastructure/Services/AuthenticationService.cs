using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HotelManagementApp.Infrastructure.Services;

public class AuthenticationService(IHttpContextAccessor httpContextAccessor) : IAuthenticationService
{
    public ClaimsPrincipal? GetLoggedInUser() => httpContextAccessor.HttpContext?.User;
}
