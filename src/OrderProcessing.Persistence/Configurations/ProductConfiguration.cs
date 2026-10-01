using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.ToTable(t => t.HasCheckConstraint("CK_Product_Stock", "Stock >= 0"));

        builder.HasMany(p => p.Tags)
            .WithMany(t => t.Products)
            .UsingEntity(j => j.ToTable("ProductTag"));
    }
}