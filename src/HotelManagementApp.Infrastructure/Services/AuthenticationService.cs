using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Infrastructure.Services
{
    public class AuthenticationService(IHttpContextAccessor httpContextAccessor, IUserManager userManager) : IAuthenticationService
    {
        public async Task<UserDto?> GetLoggedInUserAsync()
        {
            string userId = httpContextAccessor.HttpContext?.User.FindFirstValue("Id")!;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = await userManager.GetUserRolesAsync(userId),
            };
            return userDto;
        }

        public bool IsUserAuthenticatedAsync()
        {
            string userId = httpContextAccessor.HttpContext?.User.FindFirstValue("Id")!;
            return userId != null;

        }
    }
}
