using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Tag> Tags { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; } // أو أياً كان اسمها الصحيح
    DbSet<Cart> Carts { get; }
    DbSet<CartItem> CartItems { get; }
    DbSet<Payment> Payments { get; }
    DbSet<IdempotencyKey> IdempotencyKeys { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}