using System;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Config
{
     public class TaskConfiguration : IEntityTypeConfiguration<Task>
     {
          public void Configure(EntityTypeBuilder<Task> builder)
          {
               builder.HasIndex(p => p.TaskType);
               builder.HasIndex(p => p.TaskOwnerId);
               builder.HasIndex(p => p.AssignedToId);
               builder.Property(x => x.TaskDate).IsRequired().HasMaxLength(250);

               builder.HasMany(o => o.TaskItems).WithOne().OnDelete(DeleteBehavior.Cascade);
          }
     }
}