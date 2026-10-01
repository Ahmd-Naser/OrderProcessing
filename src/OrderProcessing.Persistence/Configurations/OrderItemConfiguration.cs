using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderProcessing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", "Quantity > 0"));

        builder.HasOne(oi => oi.Product)
            .WithMany() 
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}