using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelManagementApp.Infrastructure.Services;

public class JwtTokenService(IConfiguration config) : ITokenService
{
    private readonly IConfiguration _config = config;

    public string GenerateIdentityToken(UserDto user)
    {
        var tokenConfig = _config.GetSection("JwtTokenConfiguration");
        string issuer = tokenConfig.GetValue<string>("Issuer") ?? string.Empty;
        string audience = tokenConfig.GetValue<string>("Audience") ?? string.Empty; ;
        string secretKey = Environment.GetEnvironmentVariable("SecretJwtKey")
            ?? tokenConfig.GetValue<string>("SecretKey")
            ?? string.Empty;
        int jwtExpireMinutes = tokenConfig.GetValue<int>("AccessTokenExpirationMinutes");
        DateTime jwtExpireDate = DateTime.Now.AddMinutes(jwtExpireMinutes);
        var symmKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(symmKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        };
        foreach (var role in user.Roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: jwtExpireDate,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateHashRefreshToken()
    {
        byte[] refreshToken = RandomNumberGenerator.GetBytes(64);
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(refreshToken);
            return Convert.ToBase64String(bytes);
        }
    }

    public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    public int GetRefreshTokenExpirationDays() => _config.GetValue<int>("TokenConfiguration:RefreshTokenExpirationDays");

    public string? GetHashRefreshToken(string refreshToken)
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(refreshToken);
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        catch { return null; }
        ;
    }
}
