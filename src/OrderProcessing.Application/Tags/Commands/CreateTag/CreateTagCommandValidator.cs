using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Tags.Commands.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Tag name cannot be null.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name is required.");
    }
}
