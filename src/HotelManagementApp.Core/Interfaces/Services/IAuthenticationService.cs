using HotelManagementApp.Core.Dtos;

namespace HotelManagementApp.Core.Interfaces.Services;

public interface IAuthenticationService
{
    Task<UserDto?> GetLoggedInUserAsync();
    bool IsUserAuthenticatedAsync();
}
