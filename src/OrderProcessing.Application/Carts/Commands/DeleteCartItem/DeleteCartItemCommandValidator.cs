

namespace OrderProcessing.Application.Carts.Commands.DeleteCartItem;

public class DeleteCartItemCommandValidator : AbstractValidator<DeleteCartItemCommand>
{
    public DeleteCartItemCommandValidator()
    {
        RuleFor(x=> x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.CartId)
            .GreaterThan(0).WithMessage("CartId must be a positive number.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be a positive number.");
    }
}
