using Microsoft.JSInterop;

namespace HotelManagementApp.Blazor.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ITokenStore _tokenStore;
        private readonly ILogger<TokenService> _logger;
        private const string AccessTokenKey = "authToken";
        private const string RefreshTokenKey = "refreshToken";

        public TokenService(IJSRuntime jsRuntime, ITokenStore tokenStore, ILogger<TokenService> logger)
        {
            _jsRuntime = jsRuntime;
            _tokenStore = tokenStore;
            _logger = logger;
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            try 
            { 
                _logger.LogDebug("🔍 GetAccessTokenAsync called");
        
                // Najpierw sprawdź pamięć (zawsze dostępna)
                if (!string.IsNullOrEmpty(_tokenStore.AccessToken))
                {
                    _logger.LogDebug("✅ Returning access token from memory store (length: {Length})", _tokenStore.AccessToken.Length);
                    return _tokenStore.AccessToken;
                }

                _logger.LogDebug("❌ No token in memory store, trying localStorage...");

                // Jeśli nie ma w pamięci, spróbuj localStorage (tylko w kontekście Blazor)
                var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", AccessTokenKey);
                if (!string.IsNullOrEmpty(token))
                {
                    _logger.LogDebug("✅ Retrieved access token from localStorage, syncing to memory (length: {Length})", token.Length);
                    _tokenStore.AccessToken = token; // Synchronizuj z pamięcią
                    return token;
                }

                _logger.LogDebug("❌ No access token found in localStorage or memory");
                return null;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogDebug("⚠️ JSRuntime not available (prerendering?), returning token from memory: {HasToken}", 
                    !string.IsNullOrEmpty(_tokenStore.AccessToken) ? "YES" : "NO");
                return _tokenStore.AccessToken; 
            }
            catch (JSException ex) 
            { 
                _logger.LogWarning(ex, "⚠️ JavaScript error accessing localStorage, returning token from memory");
                return _tokenStore.AccessToken; 
            }
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            try 
            { 
                // Najpierw sprawdź pamięć
                if (!string.IsNullOrEmpty(_tokenStore.RefreshToken))
                {
                    _logger.LogDebug("Returning refresh token from memory store");
                    return _tokenStore.RefreshToken;
                }

                // Jeśli nie ma w pamięci, spróbuj localStorage
                var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", RefreshTokenKey);
                if (!string.IsNullOrEmpty(token))
                {
                    _logger.LogDebug("Retrieved refresh token from localStorage, syncing to memory");
                    _tokenStore.RefreshToken = token; // Synchronizuj z pamięcią
                    return token;
                }

                _logger.LogDebug("No refresh token found in localStorage or memory");
                return null;
            }
            catch (InvalidOperationException ex)
            { 
                _logger.LogDebug("JSRuntime not available (prerendering?), returning refresh token from memory: {HasToken}", 
                    !string.IsNullOrEmpty(_tokenStore.RefreshToken) ? "YES" : "NO");
                return _tokenStore.RefreshToken; 
            }
            catch (JSException ex) 
            { 
                _logger.LogWarning(ex, "JavaScript error accessing localStorage, returning refresh token from memory");
                return _tokenStore.RefreshToken; 
            }
        }

        public async Task RemoveTokensAsync()
        {
            _logger.LogDebug("Removing tokens from both memory and localStorage");
            
            // Wyczyść pamięć
            _tokenStore.Clear();

            // Wyczyść localStorage
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
                _logger.LogDebug("Tokens removed from localStorage");
            }
            catch (InvalidOperationException) 
            { 
                _logger.LogDebug("JSRuntime not available, tokens cleared from memory only");
            }
            catch (JSException ex) 
            { 
                _logger.LogWarning(ex, "JavaScript error removing tokens from localStorage");
            }
        }

        public async Task SetTokensAsync(string accessToken, string refreshToken)
        {
            _logger.LogDebug("🔧 SetTokensAsync called - AccessToken: {AccessLength}, RefreshToken: {RefreshLength}", 
                accessToken?.Length ?? 0, refreshToken?.Length ?? 0);
    
            // Ustaw w pamięci (zawsze działa)
            _tokenStore.AccessToken = accessToken;
            _tokenStore.RefreshToken = refreshToken;
    
            _logger.LogDebug("✅ Tokens set in memory store");

            // Ustaw w localStorage (jeśli dostępne)
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AccessTokenKey, accessToken);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", RefreshTokenKey, refreshToken);
                _logger.LogDebug("✅ Tokens saved to localStorage");
            }
            catch (InvalidOperationException) 
            { 
                _logger.LogDebug("⚠️ JSRuntime not available, tokens saved to memory only");
            }
            catch (JSException ex) 
            { 
                _logger.LogWarning(ex, "⚠️ JavaScript error saving tokens to localStorage");
            }
        }
    }
}