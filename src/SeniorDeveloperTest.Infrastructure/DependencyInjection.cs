using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SeniorDeveloperTest.Application.Interfaces;
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

        return services;
    }
}