using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user);
        Task<RefreshToken> CreateRefreshTokenAsync(AppUser user, string ip);
        Task<RefreshToken?> RefreshTokenAsync(string token, string ip);
        Task RevokeRefreshTokenAsync(string token, string ip);
    }
}