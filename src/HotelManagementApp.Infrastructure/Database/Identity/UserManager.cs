using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;

namespace HotelManagementApp.Infrastructure.Database.Identity
{
    public class UserManager(UserManager<User> userManager) : IUserManager
    {
        public async Task<bool> ChangePasswordAsync(UserDto user, string currentPassword, string newPassword)
        {
            var dbUser = await userManager.FindByIdAsync(user.Id);
            if (dbUser == null)
                return false;
            var result = await userManager.ChangePasswordAsync(dbUser, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> CheckPasswordAsync(UserDto user, string password)
        {
            var dbUser = await userManager.FindByIdAsync(user.Id);
            if (dbUser == null)
                return false;
            return await userManager.CheckPasswordAsync(dbUser, password);
        }

        public async Task<bool> CreateAsync(UserDto user, string password)
        {
            var newUser = new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            };
            var result = await userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                foreach (string role in user.Roles)
                    await userManager.AddToRoleAsync(newUser, role);
            }
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(UserDto user)
        {
            var dbUser = await userManager.FindByIdAsync(user.Id);
            if (dbUser == null)
                return false;
            var result = await userManager.DeleteAsync(dbUser);
            return result.Succeeded;
        }

        public async Task<UserDto?> FindByEmailAsync(string email)
        {
            var dbUser = await userManager.FindByEmailAsync(email);
            if (dbUser == null)
                return null;
            var roles = await userManager.GetRolesAsync(dbUser);
            var userDto = new UserDto
            {
                Id = dbUser.Id,
                UserName = dbUser.UserName!,
                Email = dbUser.Email!,
                Roles = roles.ToList()
            };
            return userDto;
        }

        public async Task<UserDto?> FindByIdAsync(string userId)
        {
            var dbUser = await userManager.FindByIdAsync(userId);
            if (dbUser == null)
                return null;
            var roles = await userManager.GetRolesAsync(dbUser);
            var userDto = new UserDto
            {
                Id = dbUser.Id,
                UserName = dbUser.UserName!,
                Email = dbUser.Email!,
                Roles = roles.ToList()
            };
            return userDto;
        }

        public async Task<UserDto?> FindByNameAsync(string userName)
        {
            var dbUser = await userManager.FindByNameAsync(userName);
            if (dbUser == null)
                return null;
            var roles = await userManager.GetRolesAsync(dbUser);
            var userDto = new UserDto
            {
                Id = dbUser.Id,
                UserName = dbUser.UserName!,
                Email = dbUser.Email!,
                Roles = roles.ToList()
            };
            return userDto;
        }

        public async Task<bool> UpdateAsync(UserDto user)
        {
            var dbUser = await userManager.FindByIdAsync(user.Id);
            if (dbUser == null)
                return false;
            dbUser.UserName = user.UserName;
            dbUser.Email = user.Email;
            var result = await userManager.UpdateAsync(dbUser);
            return result.Succeeded;
        }

        public async Task<ICollection<UserDto>> GetUsersInRoleAsync(string role)
        {
            var userDtos = new List<UserDto>();
            var users = await userManager.GetUsersInRoleAsync(role);
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    Roles = roles.ToList()
                };
                userDtos.Add(userDto);
            }
            return userDtos;
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return false;
            return await userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return new List<string>();
            var result = await userManager.GetRolesAsync(user);
            return result.ToList();
        }
    }
}
