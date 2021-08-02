using System.Linq;
using System.Threading.Tasks;
using core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace infra.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string role = "Admin";
 
            var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(role));
                }

            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ramesh",
                    Email = "ramesh@test.com",
                    UserName = "ramesh@test.com",
                    Address = new Address
                    {   FirstName = "Bob", SecondName = "Bobbity", Add = "10 The street", StreetAdd = "no address",
                        City = "New York", State = "NY", Pin = "90210" }
                };

                IdentityResult identityResult = await userManager.CreateAsync(user, "Pa$$w0rd");
                //await userManager.CreateAsync(user, "Pa$$w0rd");
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}