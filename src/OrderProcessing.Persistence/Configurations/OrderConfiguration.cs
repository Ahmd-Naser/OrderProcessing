using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderProcessing.Domain.Entities;


namespace OrderProcessing.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        // ربط الـ UserId (مهم لو هتعمل Index عليه بعدين للسرعة)
        builder.Property(o => o.UserId)
            .IsRequired()
            .HasMaxLength(450); // الطول القياسي لـ Identity User Id

        // ضبط نوع السعر لمنع مشاكل التقريب في الـ SQL
        builder.Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)");

        // تحويل حالة الطلب لنص في الداتابيز (Pending, Shipped...)
        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        // علاقة 1-to-Many مع الـ OrderItems
        // مش محتاجين نكتبها لأن EF Core بيفهمها لوحده، بس كتابتها بتوثق الكود
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // لو الأوردر اتمسح، امسح تفاصيله

        // علاقة 1-to-1 مع الـ Payment
        builder.HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}