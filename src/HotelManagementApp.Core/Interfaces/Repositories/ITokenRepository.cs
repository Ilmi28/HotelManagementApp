using HotelManagementApp.Core.Models;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface ITokenRepository
{
    Task AddToken(RefreshToken token);
    Task<RefreshToken?> GetToken(string refreshToken);
    Task<RefreshToken?> GetLastValidToken(string userId);
    Task RevokeToken(RefreshToken token);
}
