using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SeniorDeveloperTest.Application.Services.ExchangeRate.Interfaces;
using SeniorDeveloperTest.Application.Services.Product.Interfaces;
using SeniorDeveloperTest.Infrastructure.ExternalServices.ExchangeRate;
using SeniorDeveloperTest.Infrastructure.Persistence;
using SeniorDeveloperTest.Infrastructure.Repositories;

namespace SeniorDeveloperTest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(
            "DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "DefaultConnection is not configured.");

        services.AddSingleton(
            new SqlConnectionFactory(connectionString));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddHttpClient<IExchangeRateService, ExchangeRateClient>(client =>
        {
            client.BaseAddress = new Uri("https://open.er-api.com/v6/");
        });

        return services;
    }
}