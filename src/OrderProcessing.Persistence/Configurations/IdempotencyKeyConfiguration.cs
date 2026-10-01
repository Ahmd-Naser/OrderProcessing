using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Persistence.Configurations;

public class IdempotencyKeyConfiguration : IEntityTypeConfiguration<IdempotencyKey>
{
    public void Configure(EntityTypeBuilder<IdempotencyKey> builder)
    {
        builder.HasKey(ik => ik.Id);

        builder.Property(ik => ik.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ik => ik.RequestName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(ik => ik.Key)
            .IsUnique();
    }
}