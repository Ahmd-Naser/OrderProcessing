using System.IdentityModel.Tokens.Jwt;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Text;

namespace OrderProcessing.Infrastructure.Authentication;

public class JwtProvider(
    JwtOptions options
)
{
    private readonly JwtOptions _options = options;

    public (string Token, int ExpiresIn) GenerateToken(
        string userId,
        string email,
        IEnumerable<string> roles)
    {
        // 1. Claims الأساسية
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // 2. إضافة الأدوار (Admin, Vendor, Customer) كـ Claims منفصلة
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // 3. مفتاح التشفير وبيانات التوقيع
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(_options.ExpirationInMinutes);

        // 4. بناء الـ Token
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(tokenDescriptor);

        return (token, _options.ExpirationInMinutes * 60);
    }
}