using FluentValidation;
using SeniorDeveloperTest.Application.Queries;

namespace SeniorDeveloperTest.Application.Validators.Products;

public sealed class ProductQueryValidator
    : AbstractValidator<ProductQuery>
{
    public ProductQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Search)
            .MaximumLength(200);
    }
}