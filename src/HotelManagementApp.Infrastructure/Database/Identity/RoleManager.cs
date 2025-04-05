using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Infrastructure.Database.Identity
{
    public class RoleManager : IRoleManager
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public RoleManager(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task AddToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
                await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            var identityRoles = await _roleManager.Roles.ToListAsync();
            var result = new List<string>();
            foreach (var role in identityRoles)
                result.Add(role.Name!);
            return result;
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = new List<string>();
            if (user is not null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                    result.Add(role);
            }
            return result;
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return false;
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            var identityRoles = await _roleManager.Roles.ToListAsync();
            var result = new List<string>();
            foreach (var role in identityRoles)
                result.Add(role.Name!);
            return result.Contains(roleName.Normalize());
        }

        public async Task<ICollection<UserDto>> GetUsersInRoleAsync(string role)
        {
            var userDtos = new List<UserDto>();
            var users = await _userManager.GetUsersInRoleAsync(role);
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
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
    }
}
