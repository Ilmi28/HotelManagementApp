using System.Net.Http.Headers;
using System.Net;
using HotelManagementApp.Blazor.Auth;
using Microsoft.JSInterop;

namespace HotelManagementApp.Blazor.Services
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthTokenHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Dodaj token jeśli nie ma Authorization header
            if (request.Headers.Authorization == null)
            {
                await TryAddAuthToken(request);
            }

            var response = await base.SendAsync(request, cancellationToken);

            // Jeśli 401, spróbuj odświeżyć token i powtórz żądanie
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (await TryRefreshToken(request.RequestUri))
                {
                    await TryAddAuthToken(request);
                    if (request.Headers.Authorization != null)
                    {
                        return await base.SendAsync(request, cancellationToken);
                    }
                }
            }
            
            return response;
        }

        private async Task TryAddAuthToken(HttpRequestMessage request)
        {
            try
            {
                var token = await GetTokenWithRetry();
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch
            {
                // Ignoruj błędy - żądanie pójdzie bez tokena
            }
        }

        private async Task<string?> GetTokenWithRetry(int maxAttempts = 3)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    // Sprawdź czy JavaScript jest dostępny
                    await _jsRuntime.InvokeAsync<string>("eval", "''");
                    
                    // Pobierz token z localStorage
                    var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");
                    return token;
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("JavaScript interop"))
                {
                    // JavaScript jeszcze niedostępny, poczekaj i spróbuj ponownie
                    if (i < maxAttempts - 1)
                    {
                        await Task.Delay(500 * (i + 1)); // 500ms, 1s, 1.5s
                    }
                }
                catch
                {
                    break; // Inny błąd, przerwij próby
                }
            }
            return null;
        }

        private async Task<bool> TryRefreshToken(Uri? requestUri)
        {
            try
            {
                var refreshToken = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "refreshToken");
                if (string.IsNullOrEmpty(refreshToken))
                    return false;

                using var refreshClient = new HttpClient();
                
                var baseAddress = requestUri?.GetLeftPart(UriPartial.Authority);
                if (!string.IsNullOrEmpty(baseAddress))
                {
                    refreshClient.BaseAddress = new Uri(baseAddress);
                }

                var refreshRequest = new { RefreshToken = refreshToken };
                var refreshResponse = await refreshClient.PostAsJsonAsync("api/auth/refresh", refreshRequest);
                
                if (refreshResponse.IsSuccessStatusCode)
                {
                    var result = await refreshResponse.Content.ReadFromJsonAsync<TokenResponse>();
                    if (result != null && !string.IsNullOrEmpty(result.IdentityToken) && !string.IsNullOrEmpty(result.RefreshToken))
                    {
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.IdentityToken);
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", result.RefreshToken);
                        return true;
                    }
                }
                else
                {
                    // Usuń nieprawidłowe tokeny
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
                }
            }
            catch
            {
                // Ignoruj błędy refresh
            }

            return false;
        }

        private class TokenResponse
        {
            public string IdentityToken { get; set; } = "";
            public string RefreshToken { get; set; } = "";
        }
    }
}