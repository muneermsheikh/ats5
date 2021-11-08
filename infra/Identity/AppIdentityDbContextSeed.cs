using System.Collections.Generic;
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
            
            var roles = new List<IdentityRole>()
            {
                new IdentityRole{Name="Admin"}, new IdentityRole{Name="HRExecutive"}, 
                new IdentityRole{Name="HRTrainee"}, new IdentityRole{Name="HRSupervisor"}, 
                new IdentityRole{Name="HRManager"}, new IdentityRole{Name="AdminManager"}, 
                new IdentityRole{Name="DocumentControllerAdmin"}, new IdentityRole{Name="ProcessExecutive"}, 
                new IdentityRole{Name="MedicalExecutive"}, new IdentityRole{Name="VisaExecutiveKSA"}, 
                new IdentityRole{Name="VisaExecutiveSharjah"}, new IdentityRole{Name="VisaExecutiveDubai"}, 
                new IdentityRole{Name="VisaExecutiveQatar"}, new IdentityRole{Name="VisaExecutiveOman"}, 
                new IdentityRole{Name="MedicalExecutiveGAMMCA"}, new IdentityRole{Name="MedicalExecutive"}, 
                new IdentityRole{Name="EmigrationExecutive"}, new IdentityRole{Name="AirlineExecutive"}
            };
            foreach(var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name)) await roleManager.CreateAsync(role);
            }

            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ramesh",
                    Email = "ramesh@test.com",
                    UserName = "ramesh@test.com",
                    Address = new Address
                    {   FirstName = "Ramesh", SecondName = "Tukaram", Add = "10 The street", StreetAdd = "no address",
                        City = "Mumbai", State = "MH", Pin = "400018" }
                };

                IdentityResult identityResult = await userManager.CreateAsync(user, "Pa$$w0rd");
                //await userManager.CreateAsync(user, "Pa$$w0rd");
                /*
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
                */

                user = new AppUser
                {
                    DisplayName = "Shahid",
                    Email = "shahid@test.com",
                    UserName = "shahid@test.com",
                    Address = new Address
                    {   FirstName = "Shahid", SecondName = "Yusuf", Add = "B38 Everest Building", StreetAdd = "no address",
                        City = "Mumbai", State = "MH", Pin = "400034" }
                };
                
                identityResult = await userManager.CreateAsync(user, "Pa$$w0rd");
                /*
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
                */
                if(identityResult.Succeeded) userManager.AddToRoleAsync(user, "Admin").Wait();

            }

            
        }

        private static async Task<bool> CreateRole(string rolename, RoleManager<IdentityRole> roleManager)
        {
            var roleExist = await roleManager.RoleExistsAsync(rolename);
            if (!roleExist) {
                IdentityResult roleResult = await roleManager.CreateAsync(new IdentityRole(rolename));
                return roleResult==IdentityResult.Success;
            }
            
            return false;
            
        }
    }
}