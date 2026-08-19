using Asp.Versioning.ApiExplorer;

namespace SeniorDeveloperTest.Api.Registrars;

public class MvcWebAppRegistrar : IWebApplicationRegistrar
{
    public void RegisterPipelineComponents(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.ApiVersion.ToString());
            }
        });

        app.UseCors("AllowAngularDev");
        app.UseHttpsRedirection();
        // app.UseMiddleware<ExceptionHandlingMiddleware>(); // TODO

        app.UseAuthorization();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
    }
}