using core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Config
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
     {
          public void Configure(EntityTypeBuilder<AppUser> builder)
          {
              /*
              builder.HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                
                .IsRequired()
               
               builder.HasMany(sdr => sdr.MessagesSent).WithOne().HasForeignKey(sdrid => sdrid.SenderId);
               builder.HasMany(sdr => sdr.MessagesReceived).WithOne().HasForeignKey(sdrid => sdrid.RecipientId);
               */
          }
     }
}