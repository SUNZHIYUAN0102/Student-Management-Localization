using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.Property(s => s.Text).IsRequired().HasMaxLength(500);

            builder.HasOne(s => s.Room)
                .WithMany(m => m.Messages)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
