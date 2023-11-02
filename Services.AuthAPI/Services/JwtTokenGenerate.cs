using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BackEnd.AuthAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEnd.AuthAPI.Services
{
    public interface IJwtTokenGenerate
    {
        string GenerateToken(IdentityUser user, IEnumerable<string> roles);
    }
    public class JwtTokenGenerate : IJwtTokenGenerate
    {
        private JwtOptions jwtOptions;
        private IConfiguration configuration;

        public JwtTokenGenerate(IConfiguration configuration, IOptions<JwtOptions> jwtOptions)
        {
            this.configuration = configuration;
            this.jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(IdentityUser user, IEnumerable<string> roles)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var token = new JwtSecurityToken
            (
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
