using core.Entities.Admin;
using core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infra.Data.Config
{
     public class MessageConfiguration : IEntityTypeConfiguration<Message>
     {
          public void Configure(EntityTypeBuilder<Message> builder)
          {
               builder.Property(p => p.SenderUsername).IsRequired().HasMaxLength(50);
               builder.Property(p => p.RecipientId).IsRequired();
               builder.Property(p => p.RecipientUsername).IsRequired().HasMaxLength(50);
               builder.Property(p => p.Content).IsRequired();

               builder.HasOne(u => u.Recipient).WithMany(m => m.MessagesReceived).OnDelete(DeleteBehavior.Restrict);
               builder.HasOne(U => U.Sender).WithMany(m => m.MessagesSent).OnDelete(DeleteBehavior.Restrict);

          }
     }
}