using MediatR;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Application.Common.Interfaces;

namespace OrderProcessing.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)    
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            UserId = request.UserId,
            IsActive = true
        };

        _context.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id; // رجع الـ Id
    }
}