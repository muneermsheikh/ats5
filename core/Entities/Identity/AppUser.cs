using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace core.Entities.Identity
{
     public class AppUser: IdentityUser
    {
        public string UserType {get; set;}
        public string DisplayName { get; set; }
        public Address Address { get; set; }

        //public ICollection<AppUserRole> UserRoles { get; set; }
        //public ICollection<UserLike> LikedByUsers { get; set; }
        //public ICollection<UserLike> LikedUsers { get; set; }
        //public ICollection<Message> MessagesSent { get; set; }
        //public ICollection<Message> MessagesReceived { get; set; }
    }
}