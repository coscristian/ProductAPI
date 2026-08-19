using SeniorDeveloperTest.Infrastructure;

namespace SeniorDeveloperTest.Api.Registrars;

public sealed class InfrastructureRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructure(builder.Configuration);
    }
}