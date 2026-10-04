using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Carts.Queries.GetCheckoutPreview;

public class GetCheckoutPreviewValidator : AbstractValidator<GetCheckoutPreviewQuery>
{
    public GetCheckoutPreviewValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
