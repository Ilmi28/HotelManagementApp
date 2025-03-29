using HotelManagementApp.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Interfaces.Identity
{
    public interface ITokenManager
    {
        string GenerateAccessToken(UserDto user);
        int GetRefreshTokenExpirationDays();
        string? GetHashRefreshToken(string refreshToken);
        string GenerateRefreshToken();
    }
}
