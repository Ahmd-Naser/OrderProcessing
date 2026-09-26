
namespace OrderProcessing.Application.Carts.Queries.GetAllCartItems;

public class GetCartQueryValidator : AbstractValidator<GetCartQuery>
{
    public GetCartQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
