using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderProcessing.Domain.Consts;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = DefaultRoles.AdminId,
                Name = DefaultRoles.Admin,
                NormalizedName = DefaultRoles.Admin.ToUpper(),
                ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp
            },
            new IdentityRole
            {
                Id = DefaultRoles.VendorId,
                Name = DefaultRoles.Vendor,
                NormalizedName = DefaultRoles.Vendor.ToUpper(),
                ConcurrencyStamp = DefaultRoles.VendorRoleConcurrencyStamp
            },
            new IdentityRole
            {
                Id = DefaultRoles.CustomerId,
                Name = DefaultRoles.Customer,
                NormalizedName = DefaultRoles.Customer.ToUpper(),
                ConcurrencyStamp = DefaultRoles.CustomerRoleConcurrencyStamp
            }
        );
    }
}