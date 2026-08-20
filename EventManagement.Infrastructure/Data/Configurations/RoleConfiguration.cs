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
                ConcurrencyStamp = "f16ddde6-97b0-453c-a3a4-58fa5e0fb00a",
            },
            new IdentityRole
            {
                Id = "a102a538-5768-456d-991d-c776361a9415",
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "a41176ab-3877-45f3-a75b-f1619b82f073",

            }
        );
        }


    }
}
