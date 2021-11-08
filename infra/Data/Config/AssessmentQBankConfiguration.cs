using core.Entities;
using core.Entities.MasterEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Config
{
     public class AssessmentQBankConfiguration : IEntityTypeConfiguration<AssessmentQBank>
     {
          public void Configure(EntityTypeBuilder<AssessmentQBank> builder)
          {
               builder.Property(p => p.QNo).IsRequired();
               //builder.Property(p => p.Question).HasMaxLength(200).IsRequired().HasMaxLength(50);
               builder.HasIndex(p => new {p.QNo, p.CategoryId}).IsUnique();
               builder.HasIndex(p => new {p.Question, p.CategoryId}).IsUnique();
          }
     }
}