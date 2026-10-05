using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Common.Errors;

public static class OrderErrors
{
    public static Error DuplicateRequest() =>
        new Error("Order.DuplicateRequest", $"Duplicate request.", (int)HttpStatusCodes.Conflict);
}
