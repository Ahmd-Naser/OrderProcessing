using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Persistence.Identity.Models;
using System.Reflection;


namespace OrderProcessing.Persistence.Persistence;

public class AppDbContext: IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<IdempotencyKey> IdempotencyKeys => Set<IdempotencyKey>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
