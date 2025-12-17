using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        public TokenService(IConfiguration config, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IRefreshTokenRepository refreshTokenRepo)
        {
            _refreshTokenRepo = refreshTokenRepo;
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }

        public async Task<string> CreateTokenAsync(AppUser user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.DisplayName)
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = creds,
                Issuer = _config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(AppUser user, string ip)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(randomBytes);

            var refreshToken = new RefreshToken
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ip
            };

            await _refreshTokenRepo.RevokeAllActiveByUserAsync(user, ip, refreshToken.Token);

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return refreshToken;
        }

        public async Task<RefreshToken?> RefreshTokenAsync(string token, string ip)
        {
            var refreshToken = await _refreshTokenRepo.GetAsync(token);

            if (refreshToken == null)
                return null;

            var user = refreshToken.AppUser;
            var newRefreshToken = await CreateRefreshTokenAsync(user, ip);

            return newRefreshToken;
        }

        public async Task RevokeRefreshTokenAsync(string token, string ip)
        {
            await _refreshTokenRepo.RevokeByTokenAsync(token, ip);
        }
    }
}