using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Infrastructure.Data.Configurations
{
    public class EventRegisterConfiguration : IEntityTypeConfiguration<EventRegister>
    {
        public void Configure(EntityTypeBuilder<EventRegister> builder)
        {
            builder.HasKey(er => er.Id);

            // Relationships

            builder.HasOne(er => er.Event)
                .WithMany(e => e.EventRegisters)
                .HasForeignKey(er => er.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(er => er.User)
                .WithMany(u => u.EventRegisters)
                .HasForeignKey(er => er.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
