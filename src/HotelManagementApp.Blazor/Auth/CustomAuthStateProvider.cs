using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotelManagementApp.Blazor.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenService _tokenService;

        public CustomAuthStateProvider(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            var identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(accessToken);
                    var isExpired = jwtToken.ValidTo < DateTime.UtcNow;
                    if (!isExpired)
                    {
                        identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                    }
                    else
                    {
                        await _tokenService.RemoveTokensAsync();
                    }
                }
                catch
                {
                    await _tokenService.RemoveTokensAsync();
                }
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void NotifyAuthenticationStateChanged() =>
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}