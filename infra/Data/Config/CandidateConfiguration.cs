using System;
using core.Entities.Orders;
using core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Config
{
     public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
     {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.Property(s => s.CandidateStatus).HasConversion(
                o => o.ToString(),
                o => (EnumCandidateStatus) Enum.Parse(typeof(EnumCandidateStatus), o)
            );

            builder.Property(x => x.City).IsRequired().HasMaxLength(40);
            builder.HasIndex(x => x.ApplicationNo).IsUnique().HasFilter("[ApplicationNo] > 0");
            
            //builder.HasMany(s => s.Addresses).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(s => s.UserPhones).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(s => s.UserQualifications).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(s => s.UserProfessions).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(s => s.UserPassports).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(s => s.UserAttachments).WithOne().OnDelete(DeleteBehavior.Cascade);

            //builder.Ignore(x => x.Person);  //will not be mapped in relationships

        }
     }
}