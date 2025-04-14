using HotelManagementApp.Core.Dtos;

namespace HotelManagementApp.Core.Interfaces.Services;

public interface ITokenService
{
    string GenerateIdentityToken(UserDto user);
    int GetRefreshTokenExpirationDays();
    string? GetHashRefreshToken(string refreshToken);
    string GenerateRefreshToken();
}
