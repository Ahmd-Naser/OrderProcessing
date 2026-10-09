using OrderProcessing.Application.Common.Consts;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Auth.Commands.RegisterVendor;

public class RegisterVendorCommandValidator : AbstractValidator<RegisterVendorCommand>
{
    public RegisterVendorCommandValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

        RuleFor(x => x.StoreName)
            .NotEmpty().WithMessage("Store name is required.")
            .MaximumLength(100);
    }
}
