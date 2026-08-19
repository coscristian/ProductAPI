using SeniorDeveloperTest.Api.Exceptions;

namespace SeniorDeveloperTest.Api.Registrars;

public sealed class ExceptionHandlingRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}