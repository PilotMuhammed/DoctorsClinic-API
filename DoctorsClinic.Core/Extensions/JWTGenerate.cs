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
            var tokenKey = config["TokenKey"];
            if (string.IsNullOrWhiteSpace(tokenKey))
                throw new ArgumentNullException(nameof(tokenKey), "TokenKey is missing in the configuration.");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        }

        public string GenerateJwtToken(GenerateTokenDto generate)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, generate.Id.ToString()),
                new Claim(ClaimTypes.Name, generate.User.FullName),
                new Claim(ClaimTypes.Role, generate.Role.Name),
            };

            var permissions = generate.Role.Permissions?
                .Select(p => ((int)p).ToString())
                .Concat(generate.Permissions?.Select(p => ((int)p.Permission).ToString()) ?? Enumerable.Empty<string>())
                .Distinct()
                .ToList();

            if (permissions?.Any() == true)
                claims.Add(new Claim("PermissionsId", string.Join(',', permissions)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}