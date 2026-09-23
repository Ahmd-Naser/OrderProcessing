using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Products.Commands.UpdateIsActiveProduct;

public class UpdateIsActiveProductValidator : AbstractValidator<UpdateIsActiveProductCommand>
{
    public UpdateIsActiveProductValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0).WithMessage("Product Id must be greater than 0.");

    }
}
