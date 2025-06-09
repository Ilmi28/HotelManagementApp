using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HotelManagementApp.Blazor.Auth;

namespace HotelManagementApp.Blazor.Services
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthTokenHandler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private static readonly SemaphoreSlim _refreshSemaphore = new SemaphoreSlim(1, 1);

        public AuthTokenHandler(
            ITokenService tokenService, 
            ILogger<AuthTokenHandler> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
                
                _logger.LogDebug("=== AddTokenToRequest Debug ===");
                _logger.LogDebug("Token from service: {HasToken}", !string.IsNullOrEmpty(token) ? "EXISTS" : "NULL");
                
                if (!string.IsNullOrEmpty(token))
                {
                    _logger.LogDebug("Token length: {Length}", token.Length);
                    _logger.LogDebug("Token preview: {Preview}", token.Length > 50 ? token.Substring(0, 50) + "..." : token);
                    
                    // Sprawdź czy token nie wygasł
                    if (IsTokenExpired(token))
                    {
                        _logger.LogInformation("Token is expired, attempting to refresh");
                        var refreshed = await TryRefreshToken();
                        
                        if (refreshed)
                        {
                            // Pobierz nowy token
                            token = await _tokenService.GetAccessTokenAsync();
                            _logger.LogDebug("New token after refresh: {HasToken}", !string.IsNullOrEmpty(token) ? "EXISTS" : "NULL");
                        }
                    }

                    if (!string.IsNullOrEmpty(token))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        _logger.LogDebug("Authorization header set for request: {Uri}", request.RequestUri);
                        _logger.LogDebug("Auth header value: Bearer {TokenPreview}", token.Length > 20 ? token.Substring(0, 20) + "..." : token);
                    }
                    else
                    {
                        _logger.LogWarning("Token is null after processing for request: {Uri}", request.RequestUri);
                    }
                }
                else
                {
                    _logger.LogWarning("No access token available for request: {Uri}", request.RequestUri);
                }
                
                _logger.LogDebug("=== End AddTokenToRequest Debug ===");
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

                _logger.LogInformation("Attempting to refresh token with refresh token: {RefreshTokenPreview}", 
                    refreshToken.Length > 10 ? refreshToken[..10] + "..." : refreshToken);

                // Utwórz HttpClient bez AuthTokenHandler (aby uniknąć nieskończonej pętli)
                using var httpClient = _httpClientFactory.CreateClient();
                
                // Usuń domyślny BaseAddress jeśli jest ustawiony
                var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7227/";
                httpClient.BaseAddress = new Uri(apiBaseUrl);
                
                var refreshRequest = new
                {
                    RefreshToken = refreshToken
                };

                var json = JsonSerializer.Serialize(refreshRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogDebug("Sending refresh request to: {Url}", $"{apiBaseUrl}api/auth/refresh");
                
                var response = await httpClient.PostAsync("api/auth/refresh", content);
                
                _logger.LogDebug("Refresh response status: {StatusCode}", response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogDebug("Refresh response content: {Content}", responseContent);
                    
                    var result = JsonSerializer.Deserialize<TokenRefreshResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    if (result != null && !string.IsNullOrEmpty(result.IdentityToken))
                    {
                        // Użyj nowego refresh token jeśli został zwrócony, w przeciwnym razie zachowaj stary
                        var newRefreshToken = !string.IsNullOrEmpty(result.RefreshToken) ? result.RefreshToken : refreshToken;
                        
                        await _tokenService.SetTokensAsync(result.IdentityToken, newRefreshToken);
                        _logger.LogInformation("Token refreshed successfully");
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning("Refresh response missing required tokens");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Token refresh failed with status: {StatusCode}, content: {Content}", 
                        response.StatusCode, errorContent);
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
            _logger.LogInformation("Clearing tokens due to refresh failure");
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
                    _logger.LogWarning("Cannot read token format");
                    return true;
                }

                var jwtToken = tokenHandler.ReadJwtToken(token);
                
                // Sprawdź czy token wygasł (z 30-sekundowym buforem)
                var expirationTime = jwtToken.ValidTo;
                var currentTime = DateTime.UtcNow.AddSeconds(30); // 30-sekundowy bufor
                
                var isExpired = currentTime >= expirationTime;
                
                _logger.LogDebug("Token expires at {ExpirationTime} UTC, current time: {CurrentTime} UTC, considered expired: {IsExpired}", 
                    expirationTime, DateTime.UtcNow, isExpired);

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
                   path.Contains("/api/auth/refresh") ||
                   path.Contains("/api/auth/logout");
        }

        private class TokenRefreshResponse
        {
            public string? IdentityToken { get; set; }
            public string? RefreshToken { get; set; }
        }
    }
}