using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Application.CQRS.Auth.RefreshToken;
using HotelManagementApp.Application.CQRS.Auth.RegisterUser;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database;
using HotelManagementApp.Infrastructure.Database.Identity;
using HotelManagementApp.IntegrationTests.WebApplicationFactories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace HotelManagementApp.IntegrationTests.Tests;

public class AuthTests : IClassFixture<AuthWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly AppDbContext _context;
    private readonly UserManager<User> _manager;
    private readonly ITokenService _tokenManager;
    public AuthTests(AuthWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        var scope = factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _manager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        _tokenManager = scope.ServiceProvider.GetRequiredService<ITokenService>();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        SeedData();
    }

    private void SeedData()
    {
        var user = new User
        {
            Id = "123",
            UserName = "test",
            Email = "test@gmail.com"
        };

        var token = new RefreshToken
        {
            Id = 1,
            RefreshTokenHash = _tokenManager.GetHashRefreshToken("Xj4z8x+7Q0A=")!,
            ExpirationDate = DateTime.Now.AddDays(1),
            UserId = user.Id
        };
        var token1 = new RefreshToken
        {
            Id = 2,
            RefreshTokenHash = _tokenManager.GetHashRefreshToken("K3N5TzFhMkM=")!,
            ExpirationDate = DateTime.Now.AddDays(-1),
            UserId = user.Id
        };
        var token2 = new RefreshToken
        {
            Id = 3,
            RefreshTokenHash = _tokenManager.GetHashRefreshToken("T1hKQ1VmcDg=")!,
            ExpirationDate = DateTime.Now.AddDays(1),
            UserId = user.Id,
            IsRevoked = true
        };
        _manager.CreateAsync(user, "Password123@").Wait();
        _context.RefreshTokens.Add(token);
        _context.RefreshTokens.Add(token1);
        _context.RefreshTokens.Add(token2);
        _context.SaveChanges();
    }

    [Fact]
    public async Task RegisterUser_ValidUser_ReturnsOKAndTokens()
    {
        var cmd = new RegisterUserCommand
        {
            UserName = "test1",
            Email = "test1@gmail.com",
            Password = "Password123@"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", cmd);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);

        string? accessToken = json.RootElement.GetProperty("identityToken").GetString();
        string? refreshToken = json.RootElement.GetProperty("refreshToken").GetString();
        int userCount = _context.Users.Count();
        int tokenCount = _context.RefreshTokens.Count();
        int userLogsCount = _context.AccountLogs.Count();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrEmpty(accessToken));
        Assert.False(string.IsNullOrEmpty(refreshToken));
        Assert.Equal(2, userCount);
        Assert.Equal(4, tokenCount);
        Assert.Equal(1, userLogsCount);
    }

    [Theory]
    [InlineData("test", "test1@gmail.com", "User with this username already exists")]
    [InlineData("test1", "test@gmail.com", "User with this email already exists")]
    public async Task RegisterUser_UserAlreadyExists_ReturnsBadRequest(string username, string email, string errorMessage)
    {
        var cmd = new RegisterUserCommand
        {
            UserName = username,
            Email = email,
            Password = "Password123@"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", cmd);
        var errorMessages = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Contains(errorMessage, errorMessages);
    }

    [Fact]
    public async Task RegisterUser_NullRequest_ReturnsBadRequest()
    {
        RegisterUserCommand? cmd = null;

        var response = await _client.PostAsJsonAsync("/api/auth/register", cmd);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("a", "test1@gmail.com", "Password123@")]
    [InlineData("test1", "test1gmail.com", "Password123@")]
    [InlineData("test1", "test1@gmail.com", "password")]
    public async Task RegisterUser_InvalidInput_ReturnsBadRequest(string username, string email, string password)
    {
        RegisterUserCommand cmd = new()
        {
            UserName = username,
            Email = email,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", cmd);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task LoginUser_ValidInput_ReturnsOK()
    {
        LoginUserCommand cmd = new()
        {
            Email = "test@gmail.com",
            Password = "Password123@"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", cmd);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        string? accessToken = json.RootElement.GetProperty("identityToken").GetString();
        string? refreshToken = json.RootElement.GetProperty("refreshToken").GetString();
        int tokenCount = _context.RefreshTokens.Count();
        int revokedTokenCount = _context.RefreshTokens.Where(x => x.IsRevoked).Count();
        int userLogsCount = _context.AccountLogs.Count();


        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrEmpty(accessToken));
        Assert.False(string.IsNullOrEmpty(refreshToken));
        Assert.Equal(4, tokenCount);
        Assert.Equal(1, userLogsCount);
        Assert.Equal(2, revokedTokenCount);
    }

    [Fact]
    public async Task LoginUser_NullRequest_ReturnsBadRequest()
    {
        LoginUserCommand? cmd = null;

        var response = await _client.PostAsJsonAsync("/api/auth/login", cmd);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("test1@gmail.com", "Password123@")]
    [InlineData("test@gmail.com", "Password123!")]
    public async Task LoginUser_InvalidUserIdentifierOrPassword_ReturnsUnauthorized(string userIdentifier, string password)
    {
        LoginUserCommand cmd = new LoginUserCommand()
        {
            Email = userIdentifier,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", cmd);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RefreshToken_ValidRequest_ReturnsOK()
    {
        RefreshTokenCommand cmd = new RefreshTokenCommand
        {
            RefreshToken = "Xj4z8x+7Q0A="
        };

        var response = await _client.PostAsJsonAsync("/api/auth/refresh", cmd);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        string? accessToken = json.RootElement.GetProperty("identityToken").GetString();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrEmpty(accessToken));
    }

    [Fact]
    public async Task RefreshToken_NullRequest_ReturnsBadRequest()
    {
        RefreshTokenCommand? cmd = null;

        var response = await _client.PostAsJsonAsync("/api/auth/refresh", cmd);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("fFdqL3Y2U0g=")]
    [InlineData("K3N5TzFhMkM=")]
    [InlineData("T1hKQ1VmcDg=")]
    public async Task RefreshToken_InvalidRefreshToken_ReturnsUnauthorized(string refreshToken)
    {
        RefreshTokenCommand cmd = new RefreshTokenCommand
        {
            RefreshToken = refreshToken
        };

        var response = await _client.PostAsJsonAsync("/api/auth/refresh", cmd);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
