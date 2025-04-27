using HotelManagementApp.Core.Dtos;

namespace HotelManagementApp.Core.Interfaces.Services;

public interface ITokenService
{
    string GenerateIdentityToken(UserDto user);
    int GetRefreshTokenExpirationDays();
    string? GetTokenHash(string refreshToken);
    string Generate512Token();
}
