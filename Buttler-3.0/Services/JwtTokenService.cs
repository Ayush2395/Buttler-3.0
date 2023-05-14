using Buttler.Application.DTO;
using Buttler.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Buttler_3._0.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtHandler _jwt;

        public JwtTokenService(UserManager<AppUser> userManager, IOptions<JwtHandler> options)
        {
            _userManager = userManager;
            _jwt = options.Value;
        }

        public JwtSecurityToken CreateJwtSecurityTokenHandler(LoginDto login)
        {
            var key = Credentials();
            var claims = GetClaims(login);
            var token = new JwtSecurityToken(
                _jwt.Issuer,
                _jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: key
                );
            return token;
        }

        private List<Claim> GetClaims(LoginDto login)
        {
            var user = _userManager.FindByEmailAsync(login.Email).Result;
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,_jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Iat,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,DateTime.Now.ToString()),
                new Claim("Email",user.Email),
                new Claim("UserName",user.UserName)
            };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials Credentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
    }

    public interface IJwtTokenService
    {
        JwtSecurityToken CreateJwtSecurityTokenHandler(LoginDto login);
    }
}
