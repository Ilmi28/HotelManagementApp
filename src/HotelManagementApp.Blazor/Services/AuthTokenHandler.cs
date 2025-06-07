using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using HotelManagementApp.Blazor.Auth;

namespace HotelManagementApp.Blazor.Services
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthTokenHandler> _logger;
        private static readonly SemaphoreSlim _refreshSemaphore = new SemaphoreSlim(1, 1);

        public AuthTokenHandler(ITokenService tokenService, ILogger<AuthTokenHandler> logger)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Sprawdź czy żądanie nie dotyczy endpointów auth (logowanie/rejestracja)
            if (IsAuthEndpoint(request.RequestUri))
            {
                _logger.LogDebug("Skipping token for auth endpoint: {Uri}", request.RequestUri);
                return await base.SendAsync(request, cancellationToken);
            }

            // Dodaj token do żądania
            await AddTokenToRequest(request);

            // Wyślij żądanie
            var response = await base.SendAsync(request, cancellationToken);

            // Jeśli otrzymaliśmy 401 (Unauthorized), spróbuj odświeżyć token
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogInformation("Received 401, attempting to refresh token");
                
                var refreshed = await TryRefreshToken();
                if (refreshed)
                {
                    _logger.LogInformation("Token refreshed successfully, retrying request");
                    
                    // Dodaj nowy token do żądania i spróbuj ponownie
                    await AddTokenToRequest(request);
                    response = await base.SendAsync(request, cancellationToken);
                }
                else
                {
                    _logger.LogWarning("Token refresh failed");
                }
            }

            return response;
        }

        private async Task AddTokenToRequest(HttpRequestMessage request)
        {
            try
            {
                var token = await _tokenService.GetAccessTokenAsync();
                
                if (!string.IsNullOrEmpty(token))
                {
                    // Sprawdź czy token nie wygasł
                    if (IsTokenExpired(token))
                    {
                        _logger.LogInformation("Token is expired, attempting to refresh");
                        var refreshed = await TryRefreshToken();
                        
                        if (refreshed)
                        {
                            // Pobierz nowy token
                            token = await _tokenService.GetAccessTokenAsync();
                        }
                    }

                    if (!string.IsNullOrEmpty(token))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        _logger.LogDebug("Added Bearer token to request: {Uri}", request.RequestUri);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding token to request");
            }
        }

        private async Task<bool> TryRefreshToken()
        {
            // Używamy SemaphoreSlim aby zapobiec wielokrotnym równoczesnym odświeżeniom
            await _refreshSemaphore.WaitAsync();
            
            try
            {
                var refreshToken = await _tokenService.GetRefreshTokenAsync();
                
                if (string.IsNullOrEmpty(refreshToken))
                {
                    _logger.LogWarning("No refresh token available");
                    return false;
                }

                _logger.LogInformation("Attempting to refresh token");

                // Utwórz nowe żądanie odświeżenia tokenu
                using var httpClient = new HttpClient();
                
                // UWAGA: Tutaj używamy bezpośrednio HttpClient bez AuthTokenHandler
                // aby uniknąć nieskończonej pętli
                httpClient.BaseAddress = new Uri("https://localhost:7051/"); // Dostosuj do swojego API
                
                var refreshRequest = new
                {
                    RefreshToken = refreshToken
                };

                var response = await httpClient.PostAsJsonAsync("api/auth/refresh", refreshRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TokenRefreshResponse>();
                    
                    if (result != null && !string.IsNullOrEmpty(result.IdentityToken))
                    {
                        await _tokenService.SetTokensAsync(result.IdentityToken, result.RefreshToken ?? refreshToken);
                        _logger.LogInformation("Token refreshed successfully");
                        return true;
                    }
                }
                else
                {
                    _logger.LogWarning("Token refresh failed with status: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during token refresh");
            }
            finally
            {
                _refreshSemaphore.Release();
            }

            // Jeśli odświeżenie nie powiodło się, wyczyść tokeny
            await _tokenService.RemoveTokensAsync();
            return false;
        }

        private bool IsTokenExpired(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                
                if (!tokenHandler.CanReadToken(token))
                {
                    return true;
                }

                var jwtToken = tokenHandler.ReadJwtToken(token);
                
                // Sprawdź czy token wygasł (z 30-sekundowym buforem)
                var expirationTime = jwtToken.ValidTo;
                var currentTime = DateTime.UtcNow.AddSeconds(30); // 30-sekundowy bufor
                
                var isExpired = currentTime >= expirationTime;
                
                if (isExpired)
                {
                    _logger.LogInformation("Token expires at {ExpirationTime}, current time: {CurrentTime}, considered expired: {IsExpired}", 
                        expirationTime, DateTime.UtcNow, isExpired);
                }

                return isExpired;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking token expiration");
                return true; // Jeśli nie można sprawdzić, zakładamy że wygasł
            }
        }

        private static bool IsAuthEndpoint(Uri? uri)
        {
            if (uri == null) return false;
            
            var path = uri.AbsolutePath.ToLowerInvariant();
            return path.Contains("/api/auth/login") || 
                   path.Contains("/api/auth/register") ||
                   path.Contains("/api/auth/refresh");
        }

        private class TokenRefreshResponse
        {
            public string? IdentityToken { get; set; }
            public string? RefreshToken { get; set; }
        }
    }
}