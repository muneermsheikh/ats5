using core.Entities;
using core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Config
{
     public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
     {
          public void Configure(EntityTypeBuilder<AppUserRole> builder)
          {
               //builder.HasNoKey();
               
          }
     }
}