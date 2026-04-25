using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GarageManagement.API.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GarageManagement.API.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var jwtSection = _configuration.GetSection("JwtSettings");
        var secret = jwtSection["Secret"] ?? string.Empty;
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];
        var expiryHours = int.TryParse(jwtSection["ExpiryHours"], out var parsedExpiryHours)
            ? parsedExpiryHours
            : 24;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim("Role", user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expiryHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}