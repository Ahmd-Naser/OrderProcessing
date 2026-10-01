using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(p => p.TransactionId)
            .HasMaxLength(255)
            .IsRequired(false); // ممكن يكون Null في البداية لحد ما الدفع يتم

        // تحويل حالة الدفع لنص
        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(50);
    }
}