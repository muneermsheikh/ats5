using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.Entities.Identity
{
    //[NotMapped]
    public class AppRole: IdentityRole<int>
    {
        //[NotMapped]
        public ICollection<AppUserRole> UserRoles {get; set; }
    }
}
