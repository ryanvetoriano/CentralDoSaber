using CentralDoSaber.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CentralDoSaber.API.HealthChecks;

/// <summary>
/// Registro e mapeamento dos health checks, fora do Program.cs.
/// </summary>
public static class HealthChecksExtensions
{
    public const string HealthPath = "/health";

    public static IServiceCollection AddCentralDoSaberHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient(ExternalUrlHealthCheck.HttpClientName, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        var healthChecks = services.AddHealthChecks()
            // Processo no ar
            .AddCheck<SelfHealthCheck>("self", tags: ["live"])
            // Banco Oracle via DbContext do CP2 (Database.CanConnectAsync)
            .AddDbContextCheck<CentralDoSaberContext>(
                name: "oracle-db",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["db", "oracle"]);

        // Dependência externa (recomendado): falha vira Degraded (200), não Unhealthy (503)
        var externalUrl = configuration["HealthChecks:ExternalUrl"];
        if (Uri.TryCreate(externalUrl, UriKind.Absolute, out var uri))
        {
            healthChecks.Add(new HealthCheckRegistration(
                "fiap-site",
                sp => new ExternalUrlHealthCheck(sp.GetRequiredService<IHttpClientFactory>(), uri),
                HealthStatus.Degraded,
                ["external"],
                TimeSpan.FromSeconds(10)));
        }

        return services;
    }

    public static IEndpointConventionBuilder MapCentralDoSaberHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapHealthChecks(HealthPath, new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter.WriteAsync,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });
    }
}
