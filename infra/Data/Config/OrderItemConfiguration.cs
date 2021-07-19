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
          
               builder.OwnsOne(o => o.JobDescription , a => 
               { a.WithOwner(); });

               builder.OwnsOne(o => o.Remuneration , a => 
               { a.WithOwner(); });

               //builder.HasOne(p => p.Order).WithMany().HasForeignKey(p => p.OrderId);
          }
     }
}