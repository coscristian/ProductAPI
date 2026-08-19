using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SeniorDeveloperTest.Application.Interfaces;
using SeniorDeveloperTest.Application.Services;

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