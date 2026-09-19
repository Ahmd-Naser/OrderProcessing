namespace OrderProcessing.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    IQueryable<Domain.Entities.Product> Products { get; }
    IQueryable<Domain.Entities.Tag> Tags { get; }
    IQueryable<Domain.Entities.Cart> Carts { get; }
    IQueryable<Domain.Entities.Order> Orders { get; }
    IQueryable<Domain.Entities.OrderItem> OrderItems { get; }
    IQueryable<Domain.Entities.Payment> Payments { get; }
    IQueryable<Domain.Entities.IdempotencyKey> IdempotencyKeys { get; }

    void Add<T>(T entity) where T : class;
    void Remove<T>(T entity) where T : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}