using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            // Relationships
            builder.HasOne(p => p.EventRegister)
                .WithMany(er => er.Payments)
                .HasForeignKey(p => p.EventRegisterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Amount)
               .IsRequired()
               .HasColumnType("decimal(18,2)");
        }
    }
}
