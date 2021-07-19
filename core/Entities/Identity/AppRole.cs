using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.Entities.Users
{
    [NotMapped]
    public class AppRole: IdentityRole
    {
        public ICollection<AppUserRole> UserRoles {get; set; }
    }
}
