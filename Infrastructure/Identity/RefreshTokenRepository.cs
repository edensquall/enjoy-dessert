using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppIdentityDbContext _context;
        public RefreshTokenRepository(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetAsync(string token)
        {
            var now = DateTime.UtcNow;

            return await _context.RefreshTokens
                .Include(x => x.AppUser)
                .FirstOrDefaultAsync(x =>
                    x.Token == token &&
                    x.RevokedAt == null &&
                    x.ExpiresAt > now);
        }

        public async Task<List<RefreshToken>> GetAllActiveByUserAsync(AppUser user)
        {
            var now = DateTime.UtcNow;

            return await _context.RefreshTokens
                .Where(x =>
                    x.AppUserId == user.Id &&
                    x.RevokedAt == null &&
                    x.ExpiresAt > now
                )
                .ToListAsync();
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeAllActiveByUserAsync(AppUser user, string revokedByIp, string replacedByToken)
        {
            var now = DateTime.UtcNow;

            await _context.RefreshTokens
                .Where(x =>
                    x.AppUserId == user.Id &&
                    x.RevokedAt == null &&
                    x.ExpiresAt > now
                ).ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.RevokedAt, now)
                    .SetProperty(u => u.RevokedByIp, revokedByIp)
                    .SetProperty(u => u.ReplacedByToken, replacedByToken)
                );
        }

        public async Task RevokeByTokenAsync(string token, string revokedByIp)
        {
            var now = DateTime.UtcNow;

            await _context.RefreshTokens
                .Where(x =>
                    x.Token == token
                ).ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.RevokedAt, now)
                    .SetProperty(u => u.RevokedByIp, revokedByIp)
                );
        }

    }
}