using SeniorDeveloperTest.Application;

namespace SeniorDeveloperTest.Api.Registrars;

public sealed class ApplicationRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddApplication();
    }
}