﻿using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Database.Identity;

public class UserManager(UserManager<User> userManager) : IUserManager, IUserRolesManager
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
            Roles = roles.ToList(),
            IsEmailConfirmed = dbUser.EmailConfirmed
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
            Roles = roles.ToList(),
            IsEmailConfirmed = dbUser.EmailConfirmed
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
            Roles = roles.ToList(),
            IsEmailConfirmed = dbUser.EmailConfirmed
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
        dbUser.EmailConfirmed = user.IsEmailConfirmed;
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
                Roles = roles.ToList(),
                IsEmailConfirmed = user.EmailConfirmed
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

    public async Task<ICollection<string>> GetUserRolesAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return [];
        var result = await userManager.GetRolesAsync(user);
        return result.ToList();
    }

    public async Task<bool> AddToRoleAsync(UserDto user, string role)
    {
        var dbUser = await userManager.FindByIdAsync(user.Id);
        if (dbUser == null)
            return false;
        var result = await userManager.AddToRoleAsync(dbUser, role);
        return result.Succeeded;
    }

    public async Task<bool> RemoveFromRoleAsync(UserDto user, string role)
    {
        var dbUser = await userManager.FindByIdAsync(user.Id);
        if (dbUser == null)
            return false;
        var result = await userManager.RemoveFromRoleAsync(dbUser, role);
        return result.Succeeded;
    }

    public async Task<ICollection<UserDto>> GetUsersWithoutRole()
    {
        var userDtos = new List<UserDto>();

        var users = await userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    Roles = new List<string>(),
                    IsEmailConfirmed = user.EmailConfirmed
                });
            }
        }

        return userDtos;
    }


    public async Task<bool> ResetPasswordAsync(UserDto user, string newPassword)
    {
        var dbUser = await userManager.FindByIdAsync(user.Id);
        if (dbUser == null)
            return false;
        await userManager.RemovePasswordAsync(dbUser);
        await userManager.AddPasswordAsync(dbUser, newPassword);
        return true;
    }
}
