using HotelManagementApp.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<UserDto?> GetLoggedInUserAsync();
        bool IsUserAuthenticatedAsync();
    }
}
