using core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace core.Entities.Users
{
    public class AppUserRole: IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }

    }
}
