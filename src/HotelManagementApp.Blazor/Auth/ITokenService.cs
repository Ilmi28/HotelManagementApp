namespace HotelManagementApp.Blazor.Auth
{
    public interface ITokenService
    {
        Task<string?> GetAccessTokenAsync();
        Task<string?> GetRefreshTokenAsync();
        Task SetTokensAsync(string accessToken, string refreshToken);
        Task RemoveTokensAsync();
    }
}