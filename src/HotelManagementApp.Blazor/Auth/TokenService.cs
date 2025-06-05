using Microsoft.JSInterop;

namespace HotelManagementApp.Blazor.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string AccessTokenKey = "authToken";
        private const string RefreshTokenKey = "refreshToken";

        public TokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // Należy dodać obsługę błędów i przypadku prerenderingu (gdy JS nie jest dostępny)
        public async Task<string?> GetAccessTokenAsync()
        {
            try { return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", AccessTokenKey); }
            catch (InvalidOperationException) { return null; } // Prerendering
            catch (JSException) { return null; } // Błąd JS
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            try { return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", RefreshTokenKey); }
            catch (InvalidOperationException) { return null; }
            catch (JSException) { return null; }
        }

        public async Task RemoveTokensAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
            }
            catch (InvalidOperationException) { } // Ignoruj błąd prerenderingu
            catch (JSException) { } // Ignoruj błąd JS
        }

        public async Task SetTokensAsync(string accessToken, string refreshToken)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AccessTokenKey, accessToken);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", RefreshTokenKey, refreshToken);
            }
            catch (InvalidOperationException) { }
            catch (JSException) { }
        }
    }
}