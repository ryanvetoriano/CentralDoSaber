using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CentralDoSaber.API.HealthChecks;

/// <summary>
/// Se este código executa, o processo está no ar.
/// </summary>
public class SelfHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("API em execução."));
    }
}
