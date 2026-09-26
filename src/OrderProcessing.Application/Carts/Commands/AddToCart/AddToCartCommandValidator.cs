using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Carts.Commands.AddToCart;

public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator()
    {
        
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be a positive number.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be a positive number.");
    }
}
