using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Http; // ✅ Dodaj to dla HttpClientFactoryOptions
using HotelManagementApp.Blazor.Services;
using HotelManagementApp.Blazor.Auth;

var builder = WebApplication.CreateBuilder(args);

// --- Konfiguracja Odczytu Ustawień ---
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? builder.Configuration["BaseUrl"] ?? "https://localhost:7227/";

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

builder.Services.AddSingleton<ITokenStore, InMemoryTokenStore>();
builder.Services.AddScoped<ITokenService, TokenService>();

// ✅ WAŻNE: Zarejestruj AuthTokenHandler jako Transient (potrzebne dla każdego HttpClient)
builder.Services.AddTransient<AuthTokenHandler>();

// Rejestracja usługi do komunikacji między komponentami
builder.Services.AddSingleton<IEventBusService, EventBusService>();

// ✅ HttpClient Factory - podstawowy do tworzenia różnych klientów
builder.Services.AddHttpClient();

// ✅ HttpClient dla refresh tokenów (BEZ AuthTokenHandler, żeby uniknąć nieskończonej pętli)
builder.Services.AddHttpClient("RefreshClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// ✅ HttpClient dla normalnych operacji API (Z AuthTokenHandler)
builder.Services.AddHttpClient("HotelApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
}).AddHttpMessageHandler<AuthTokenHandler>();

// ✅ WAŻNE: Domyślny HttpClient dla DI - używa HotelApi (z AuthTokenHandler)
builder.Services.AddScoped<HttpClient>(serviceProvider =>
{
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    return httpClientFactory.CreateClient("HotelApi");
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

// Dodaj to przed app.Run() do debugowania
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api"))
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        Console.WriteLine($"=== API Request ===");
        Console.WriteLine($"Path: {context.Request.Path}");
        Console.WriteLine($"Auth Header: {(authHeader != null ? "PRESENT" : "MISSING")}");
        Console.WriteLine($"==================");
    }
    await next();
});

// --- Uruchomienie Aplikacji ---
app.Run();