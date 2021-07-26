using core.Entities;
using core.Entities.MasterEntities;
using core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Config
{
     public class UserPhoneConfiguration : IEntityTypeConfiguration<UserPhone>
     {
          public void Configure(EntityTypeBuilder<UserPhone> builder)
          {
               builder.Property(p => p.CandidateId).IsRequired();
               builder.Property(p => p.PhoneNo).IsRequired().HasMaxLength(10);
               builder.HasIndex(p => p.PhoneNo).IsUnique();

               builder.HasIndex(x => x.CandidateId).HasFilter("[IsValid]=1");  //TODO - or should it be=True?

               //builder.HasOne(p => p.Candidate).WithMany().HasForeignKey(p => p.CandidateId);
          }
     }
}