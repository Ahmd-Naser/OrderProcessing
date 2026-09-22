using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than zero.");
    }
}
