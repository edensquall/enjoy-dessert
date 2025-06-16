using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new AppRole(nameof(RoleType.Admin)));
                await roleManager.CreateAsync(new AppRole(nameof(RoleType.RegularMember)));
                await roleManager.CreateAsync(new AppRole(nameof(RoleType.GoldMember)));
                await roleManager.CreateAsync(new AppRole(nameof(RoleType.DiamondMember)));
            }
        }

        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    UserName = "admin01",
                    DisplayName = "系統管理員",
                    PhoneNumber = "0123456789",
                    Email = "admin@example.com",
                    Address = new Address
                    {
                        FirstName = "系統",
                        LastName = "管理員",
                        County = "台北市",
                        City = "中正區",
                        Street = "忠孝東路一段1號",
                        ZipCode = "100",
                        PhoneNumber = "0123456789"
                    }
                };

                var result = await userManager.CreateAsync(user, "admin01");
                if (result.Succeeded)
                {
                    userManager.AddToRolesAsync(user, new string[] { nameof(RoleType.Admin), nameof(RoleType.RegularMember) }).Wait();
                }
            }
        }
    }
}
