using System.Security.Claims;

namespace HotelManagementApp.Core.Interfaces.Services;

public interface IAuthenticationService
{
    ClaimsPrincipal? GetLoggedInUser();
}
