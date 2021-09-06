using System;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Config
{
     public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
     {
          public void Configure(EntityTypeBuilder<TaskItem> builder)
          {
               builder.HasIndex(p => p.ApplicationTaskId);
               builder.Property(x => x.TransactionDate).IsRequired();
               builder.Property(x => x.TaskStatus).IsRequired();
               builder.Property(x => x.UserId).IsRequired();
               builder.HasIndex(x => x.UserId).HasFilter("[UserId]>0");
               builder.HasIndex(p => p.TransactionDate);
               builder.Property(x => x.TaskItemDescription).IsRequired().HasMaxLength(250);
               builder.HasOne(p => p.ApplicationTask).WithMany().HasForeignKey(p => p.ApplicationTaskId);

          }
     }
}