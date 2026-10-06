using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Common.Errors;

public static class OrderErrors
{
    public static Error NotFound(int id) =>
        new Error("Order.NotFound", $"Order with ID {id} not found", (int)HttpStatusCodes.NotFound);
    public static Error DuplicateRequest() =>
        new Error("Order.DuplicateRequest", $"Duplicate request.", (int)HttpStatusCodes.Conflict);
}
