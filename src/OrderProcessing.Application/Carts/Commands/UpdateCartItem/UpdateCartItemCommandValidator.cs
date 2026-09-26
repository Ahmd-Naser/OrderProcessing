using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Carts.Commands.UpdateCartItem;

public class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemCommandValidator()
    {
        RuleFor(x => x.CartId)
            .GreaterThan(0).WithMessage("CartId must be a positive number.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be a positive number.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be a positive number.");
    }
}
