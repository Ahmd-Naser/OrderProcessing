using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    IQueryable<Product> Products { get; }
    IQueryable<Tag> Tags { get; }
    IQueryable<Cart> Carts { get; }
    IQueryable<Order> Orders { get; }
    IQueryable<OrderItem> OrderItems { get; }
    IQueryable<Payment> Payments { get; }
    IQueryable<IdempotencyKey> IdempotencyKeys { get; }

    void Add<T>(T entity) where T : class;
    void Remove<T>(T entity) where T : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}