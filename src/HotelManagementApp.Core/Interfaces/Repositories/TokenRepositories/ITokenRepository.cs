namespace HotelManagementApp.Core.Interfaces.Repositories.TokenRepositories;

public interface ITokenRepository<TTokenType>
{
    Task AddToken(TTokenType token, CancellationToken ct = default);
    Task<TTokenType?> GetToken(string token, CancellationToken ct = default);
    Task<TTokenType?> GetTokenByUser(string userId, CancellationToken ct = default);
    Task DeleteToken(TTokenType token, CancellationToken ct = default);
}
