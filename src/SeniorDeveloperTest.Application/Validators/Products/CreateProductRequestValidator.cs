using FluentValidation;
using SeniorDeveloperTest.Application.Dtos;
using SeniorDeveloperTest.Application.Services.Product.Dtos.Products;

namespace SeniorDeveloperTest.Application.Validators.Products;

public sealed class CreateProductRequestValidator
    : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}