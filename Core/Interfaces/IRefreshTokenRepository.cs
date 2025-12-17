using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;

namespace Core.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        Task RevokeAllActiveByUserAsync(AppUser user, string revokedByIp, string replacedByToken);
        Task RevokeByTokenAsync(string token, string revokedByIp);
    }
}