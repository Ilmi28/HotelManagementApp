namespace HotelManagementApp.Core.Interfaces.Identity;

public interface IRoleManager
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<List<string>> GetAllRolesAsync();
}
