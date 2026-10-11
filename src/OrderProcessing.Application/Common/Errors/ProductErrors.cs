using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Common.Errors;

public static class ProductErrors
{
    public static Error NotFound(int productId) =>
        new Error( "Product.NotFound", $"Product with ID {productId} was not found." , (int)HttpStatusCodes.NotFound );


    public static Error Forbidden(int productId) =>
        new Error("Product.Forbidden", $"Product with ID {productId} is not accessible.", (int)HttpStatusCodes.Forbidden);

    public static Error AlreadyExists(string productName) =>
        new Error( "Product.AlreadyExists", $"Product with name '{productName}' already exists." , (int)HttpStatusCodes.Conflict );
    public static Error InvalidData() =>
        new Error("Product.InvalidData", $"Invalid product data." , (int)HttpStatusCodes.BadRequest );
}
