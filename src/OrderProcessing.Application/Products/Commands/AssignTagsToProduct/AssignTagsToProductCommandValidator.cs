using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Products.Commands.AssignTagsToProduct;

public class AssignTagsToProductCommandValidator : AbstractValidator<AssignTagsToProductCommand>
{
    public AssignTagsToProductCommandValidator()
    {
        //RuleFor(x => x.ProductId)
        //    .GreaterThan(0).WithMessage("Product Id must be greater than zero.");

        RuleFor(x => x.TagIds)
            .NotEmpty().WithMessage("Tag Ids cannot be empty.")
            .Must(tagIds => tagIds.All(id => id > 0)).WithMessage("All Tag Ids must be greater than zero.");
    }
}
