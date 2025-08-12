using DoctorsClinic.Core.Dtos.Account;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;


namespace DoctorsClinic.Core.Extensions
{
    public class JWTGenerate
    {
        private readonly SymmetricSecurityKey _key;

        public JWTGenerate(IConfiguration config)
        {
            var tokenKey = config["TokenKey"] ?? throw new InvalidOperationException("TokenKey is missing.");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        }

        public string GenerateJwtToken(GenerateTokenDto generate)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, generate.UserId.ToString()),
                new Claim(ClaimTypes.Name, generate.UserName ?? string.Empty),
                new Claim(ClaimTypes.Role, generate.RoleName ?? string.Empty),
            };

            var allPermissions = (generate.RolePermissions ?? Array.Empty<string>())
                .Concat(generate.UserPermissions ?? Array.Empty<string>())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (allPermissions.Length > 0)
            {
                claims.Add(new Claim("PermissionsId", string.Join(',', allPermissions)));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(30),

                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}