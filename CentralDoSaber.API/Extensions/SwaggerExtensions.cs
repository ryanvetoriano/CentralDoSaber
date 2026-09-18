using System.Reflection;
using Microsoft.OpenApi;

namespace CentralDoSaber.API.Extensions;

/// <summary>Metadados do documento OpenAPI (seção "Swagger" do appsettings).</summary>
public class SwaggerSettings
{
    public const string SectionName = "Swagger";

    public string Title { get; set; } = "Central do Saber API";
    public string Version { get; set; } = "v1";
    public string Description { get; set; } = string.Empty;
}

public static class SwaggerExtensions
{
    public static IServiceCollection AddCentralDoSaberSwagger(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetSection(SwaggerSettings.SectionName).Get<SwaggerSettings>()
                       ?? new SwaggerSettings();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(settings.Version, new OpenApiInfo
            {
                Title = settings.Title,
                Version = settings.Version,
                Description = settings.Description
            });

            // XML da API (controllers) e da Application (DTOs)
            IncludeXmlCommentsOf(options, Assembly.GetExecutingAssembly());
            IncludeXmlCommentsOf(options, typeof(Application.DTO.UserResponse).Assembly);
        });

        return services;
    }

    public static WebApplication UseCentralDoSaberSwagger(this WebApplication app, IConfiguration configuration)
    {
        var settings = configuration.GetSection(SwaggerSettings.SectionName).Get<SwaggerSettings>()
                       ?? new SwaggerSettings();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{settings.Version}/swagger.json", $"{settings.Title} {settings.Version}");
            options.DocumentTitle = settings.Title;
            // Permite abrir uma operação direto pela URL (ex.: /swagger#/Generos/post_api_generos)
            options.EnableDeepLinking();
        });

        return app;
    }

    private static void IncludeXmlCommentsOf(
        Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options,
        Assembly assembly)
    {
        var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);
    }
}
