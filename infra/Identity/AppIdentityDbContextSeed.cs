using System.Linq;
using System.Threading.Tasks;
using core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace infra.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
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

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}