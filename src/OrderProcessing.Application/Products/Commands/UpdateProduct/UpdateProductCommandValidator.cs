
namespace OrderProcessing.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        //RuleFor(v => v.Id)
        //    .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Product description is required.")
            .MaximumLength(1000).WithMessage("Product description must not exceed 1000 characters.");

        RuleFor(v => v.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(v => v.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        // فاليديشن مخصص لليستة الصور
        RuleFor(v => v.Pics)
            .NotNull().WithMessage("Pics list cannot be null.");

        RuleForEach(v => v.Pics)
            .NotEmpty().WithMessage("Image URL/Path cannot be empty.")
            .Must(BeAValidUrl).WithMessage("Each picture must be a valid URL or path.");
        // تقدر تشيل السطر الأخير لو هتحفظ اسم الملف بس مش URL كامل
    }

    // دالة مساعدة للتأكد إن النص المبعوت عبارة عن رابط سليم
    private bool BeAValidUrl(string url)
    {
        // لو بتحفظ مسار نسبي زي /images/pic1.jpg غير الشرط ده
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}