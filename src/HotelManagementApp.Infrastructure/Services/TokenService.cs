using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelManagementApp.Infrastructure.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string GenerateIdentityToken(UserDto user)
    {
        var tokenConfig = config.GetSection("JwtTokenConfiguration");
        string issuer = tokenConfig.GetValue<string>("Issuer") ?? string.Empty;
        string[] audience = tokenConfig.GetSection("Audience").Get<string[]>() ?? [];
        string secretKey = Environment.GetEnvironmentVariable("JwtSecretKey")
            ?? tokenConfig["SecretKey"]
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
        foreach (var aud in audience)
            claims.Add(new Claim("aud", aud));

        var token = new JwtSecurityToken(
            issuer: issuer,
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

    public string Generate512Token() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(512));

    public int GetRefreshTokenExpirationDays() => config.GetValue<int>("JwtTokenConfiguration:RefreshTokenExpirationDays");

    public string? GetTokenHash(string refreshToken)
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(refreshToken);
            using (var sha512 = SHA3_512.Create())
            {
                var hash = sha512.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        catch { return null; }
        ;
    }
}
