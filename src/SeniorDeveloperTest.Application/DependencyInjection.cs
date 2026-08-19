using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SeniorDeveloperTest.Application.Services;
using SeniorDeveloperTest.Application.Services.Product;
using SeniorDeveloperTest.Application.Services.Product.Interfaces;

namespace SeniorDeveloperTest.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddValidatorsFromAssemblyContaining<IProductService>();

        return services;
    }
}