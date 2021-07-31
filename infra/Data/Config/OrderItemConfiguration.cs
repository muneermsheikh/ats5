using System;
using core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Config
{
     public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
     {
          public void Configure(EntityTypeBuilder<OrderItem> builder)
          {
               builder.Property(s => s.Status).HasConversion(
               o => o.ToString(),
               o => (EnumOrderItemStatus) Enum.Parse(typeof(EnumOrderItemStatus), o)
               );
          
               /*builder.OwnsOne(o => o.JobDescription , a => 
               { a.WithOwner(); });

               builder.OwnsOne(o => o.Remuneration , a => 
               { a.WithOwner(); });
               */

               builder.HasOne(s => s.Remuneration).WithOne().OnDelete(DeleteBehavior.Cascade);
               builder.HasOne(x => x.JobDescription).WithOne().OnDelete(DeleteBehavior.Cascade);
               builder.HasMany(s => s.CVRefs).WithOne().OnDelete(DeleteBehavior.Cascade);
               //builder.HasOne(s => s.Category).WithOne().OnDelete(DeleteBehavior.Restrict);
          }
     }
}