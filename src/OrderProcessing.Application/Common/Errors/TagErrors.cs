using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Common.Errors;

public static class TagErrors
{
    public static Error NotFound(int tagId) =>
        new Error("Tag.NotFound", $"Tag with ID {tagId} was not found.", (int)HttpStatusCodes.NotFound);
    public static Error AlreadyExists(string tagName) =>
        new Error("Tag.AlreadyExists", $"Tag with name '{tagName}' already exists.", (int)HttpStatusCodes.Conflict);
    public static Error InvalidData() =>
        new Error("Tag.InvalidData", $"Invalid tag data.", (int)HttpStatusCodes.BadRequest);
}
