﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotelManagementApp.Blazor.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenService _tokenService;
        private readonly NavigationManager _navigationManager;
        private readonly ILogger<CustomAuthStateProvider> _logger;

        public CustomAuthStateProvider(ITokenService tokenService, NavigationManager navigationManager, ILogger<CustomAuthStateProvider> logger)
        {
            _tokenService = tokenService;
            _navigationManager = navigationManager;
            _logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _tokenService.GetAccessTokenAsync();

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogDebug("No token found, user is not authenticated");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                // Sprawdź czy token jest prawidłowy
                var tokenHandler = new JwtSecurityTokenHandler();
                if (!tokenHandler.CanReadToken(token))
                {
                    _logger.LogWarning("Invalid token format, clearing tokens");
                    await _tokenService.RemoveTokensAsync();
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Sprawdź czy token nie wygasł
                if (jwtToken.ValidTo <= DateTime.UtcNow)
                {
                    _logger.LogInformation("Token expired, attempting refresh");
                    
                    // Spróbuj odświeżyć token
                    var refreshed = await TryRefreshTokenInProvider();
                    if (!refreshed)
                    {
                        _logger.LogWarning("Token refresh failed, user will be logged out");
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    }

                    // Pobierz nowy token
                    token = await _tokenService.GetAccessTokenAsync();
                    if (string.IsNullOrEmpty(token))
                    {
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    }
                    
                    jwtToken = tokenHandler.ReadJwtToken(token);
                }

                var claims = jwtToken.Claims.ToList();
                var identity = new ClaimsIdentity(claims, "jwt");
                var principal = new ClaimsPrincipal(identity);

                _logger.LogDebug("User authenticated with {ClaimsCount} claims", claims.Count);
                return new AuthenticationState(principal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting authentication state");
                await _tokenService.RemoveTokensAsync();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        private async Task<bool> TryRefreshTokenInProvider()
        {
            try
            {
                var refreshToken = await _tokenService.GetRefreshTokenAsync();
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return false;
                }

                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:7227/"); // Dostosuj do swojego API

                var refreshRequest = new { RefreshToken = refreshToken };
                var response = await httpClient.PostAsJsonAsync("api/auth/refresh", refreshRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TokenRefreshResponse>();
                    if (result != null && !string.IsNullOrEmpty(result.IdentityToken))
                    {
                        await _tokenService.SetTokensAsync(result.IdentityToken, result.RefreshToken ?? refreshToken);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token in provider");
            }

            return false;
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task LogoutAsync()
        {
            await _tokenService.RemoveTokensAsync();
            NotifyAuthenticationStateChanged();
            _navigationManager.NavigateTo("/login", forceLoad: true);
        }

        private class TokenRefreshResponse
        {
            public string? IdentityToken { get; set; }
            public string? RefreshToken { get; set; }
        }
    }
}