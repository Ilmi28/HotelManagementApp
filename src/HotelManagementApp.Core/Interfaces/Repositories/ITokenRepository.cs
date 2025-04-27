namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface ITokenRepository<TTokenType>
{
    Task AddToken(TTokenType token, CancellationToken ct = default);
    Task<TTokenType?> GetToken(string refreshToken, CancellationToken ct = default);
    Task<TTokenType?> GetTokenByUser(string userId, CancellationToken ct = default);
    Task DeleteToken(TTokenType token, CancellationToken ct = default);
}
