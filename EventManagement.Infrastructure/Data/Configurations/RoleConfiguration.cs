using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Id = "49750c33-447c-4d6c-9abd-30718d9ba67d",
                Name = "Admin",
                NormalizedName = "ADMIN",
            },
            new IdentityRole
            {
                Id = "a102a538-5768-456d-991d-c776361a9415",
                Name = "User",
                NormalizedName = "USER",
            },
            new IdentityRole
            {
                Id = "c34d1d0b-c717-4b32-8157-da17a0b42a73",
                Name = "Organizer",
                NormalizedName = "ORGANIZER",
            }
        );
        }


    }
}
