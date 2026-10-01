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

        // ضبط نوع السعر
        builder.Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(18,2)");

        // ضمان إن الكمية دايماً أكبر من صفر (Check Constraint في SQL)
        // ده بيدي حماية إضافية على مستوى الداتابيز
        builder.ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", "Quantity > 0"));

        // علاقة الـ Item بالمنتج
        builder.HasOne(oi => oi.Product)
            .WithMany() // لأن الـ Product ملوش List<OrderItem> جواه
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // يمنع مسح منتج لو ليه أوردرات سابقة! دي نقطة جوهرية في الـ E-commerce
    }
}