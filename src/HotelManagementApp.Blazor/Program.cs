using Microsoft.AspNetCore.Components.Authorization;
using HotelManagementApp.Blazor.Services;
using HotelManagementApp.Blazor.Auth;

var builder = WebApplication.CreateBuilder(args);

// --- Konfiguracja Odczytu Ustawień ---
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? builder.Configuration["BaseUrl"] ?? "http://localhost:5075/";

// --- Dodawanie Usług do Kontenera ---

// Usługi dla Razor Pages (potrzebne dla _Host.cshtml)
builder.Services.AddRazorPages(options =>
{
    options.RootDirectory = "/Components";
});

// Usługi dla Blazor Server
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 100 * 1024; // 100 KB
    });

// --- Kluczowe dla obsługi JWT w Blazor Server ---
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<AuthTokenHandler>();

// ✅ POPRAWNA konfiguracja HttpClient z AuthTokenHandler
builder.Services.AddHttpClient("HotelApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<AuthTokenHandler>();

// ✅ WAŻNE: Zamień domyślny HttpClient na ten z AuthTokenHandler
builder.Services.AddScoped<HttpClient>(serviceProvider =>
{
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    return httpClientFactory.CreateClient("HotelApi"); // Używa klienta z AuthTokenHandler
});

// Usługi Autentykacji i Autoryzacji
builder.Services.AddAuthorizationCore();

// --- Budowanie Aplikacji ---
var app = builder.Build();

// --- Konfiguracja Potoku Przetwarzania Żądań HTTP (Middleware) ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// --- WAŻNE: Kolejność middleware dla uwierzytelniania/autoryzacji ---
app.UseAuthentication();
app.UseAuthorization();

// Mapowanie endpointów
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// --- Uruchomienie Aplikacji ---
app.Run();