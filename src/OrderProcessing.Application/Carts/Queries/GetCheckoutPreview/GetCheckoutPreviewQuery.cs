using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Carts.Queries.GetCheckoutPreview;

public record GetCheckoutPreviewQuery(
    string UserId
) : IRequest<Result<GetCheckoutPreviewResponse>>;