using System.Security.Claims;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByUserByClaimsPrincipleWithAddressAsync(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var userName = user.RetrieveUserNameFromPrincipal();

            return await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.UserName == userName);
        }

        public static async Task<AppUser> FindByUserNameFromClaimsPrinciple(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var userName = user.RetrieveUserNameFromPrincipal();

            return await input.Users.SingleOrDefaultAsync(x => x.UserName == userName);
        }
    }
}