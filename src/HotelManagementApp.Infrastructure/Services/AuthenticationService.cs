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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IUserManager userManager, IRoleManager roleManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserDto?> GetLoggedInUserAsync()
        {
            string userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("Id")!;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = await _roleManager.GetUserRolesAsync(userId),
            };
            return userDto;
        }

        public bool IsUserAuthenticatedAsync()
        {
            string userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("Id")!;
            return userId != null;

        }
    }
}
