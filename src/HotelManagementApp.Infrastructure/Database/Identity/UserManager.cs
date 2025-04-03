using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Infrastructure.Database.Identity
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<User> _userManager;
        public UserManager(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> ChangePasswordAsync(UserDto user, string currentPassword, string newPassword)
        {
            var dbUser = await _userManager.FindByIdAsync(user.Id);
            if (dbUser == null)
                return false;
            var result = await _userManager.ChangePasswordAsync(dbUser, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> CheckPasswordAsync(UserDto user, string password)
        {
            var dbUser = await _userManager.FindByIdAsync(user.Id);
            if (dbUser == null)
                return false;
            return await _userManager.CheckPasswordAsync(dbUser, password);
        }

        public async Task<bool> CreateAsync(UserDto user, string password)
        {
            var newUser = new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            };
            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                foreach (string role in user.Roles)
                    await _userManager.AddToRoleAsync(newUser, role);
            }
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(UserDto user)
        {
            var dbUser = await _userManager.FindByIdAsync(user.Id);
            if (dbUser == null)
                return false;
            var result = await _userManager.DeleteAsync(dbUser);
            return result.Succeeded;
        }

        public async Task<UserDto?> FindByEmailAsync(string email)
        {
            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser == null)
                return null;
            var roles = await _userManager.GetRolesAsync(dbUser);
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
            var dbUser = await _userManager.FindByIdAsync(userId);
            if (dbUser == null)
                return null;
            var roles = await _userManager.GetRolesAsync(dbUser);
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
            var dbUser = await _userManager.FindByNameAsync(userName);
            if (dbUser == null)
                return null;
            var roles = await _userManager.GetRolesAsync(dbUser);
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
            var dbUser = await _userManager.FindByIdAsync(user.Id);
            if (dbUser == null)
                return false;
            dbUser.UserName = user.UserName;
            dbUser.Email = user.Email;
            var result = await _userManager.UpdateAsync(dbUser);
            return result.Succeeded;
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
