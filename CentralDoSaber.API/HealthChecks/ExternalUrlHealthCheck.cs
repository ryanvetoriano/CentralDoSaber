using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CentralDoSaber.API.HealthChecks;

/// <summary>
/// Verifica se uma URL externa responde com status de sucesso.
/// Em caso de falha devolve o FailureStatus do registro
/// (Degraded, para que um terceiro fora do ar não derrube o /health inteiro).
/// </summary>
public class ExternalUrlHealthCheck : IHealthCheck
{
    public const string HttpClientName = "external-url-health-check";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Uri _url;

    public ExternalUrlHealthCheck(IHttpClientFactory httpClientFactory, Uri url)
    {
        _httpClientFactory = httpClientFactory;
        _url = url;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var data = new Dictionary<string, object> { ["url"] = _url.ToString() };

        try
        {
            var client = _httpClientFactory.CreateClient(HttpClientName);
            using var response = await client.GetAsync(
                _url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            var statusCode = (int)response.StatusCode;
            data["statusCode"] = statusCode;

            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy($"{_url.Host} respondeu {statusCode}.", data)
                : new HealthCheckResult(
                    context.Registration.FailureStatus,
                    $"{_url.Host} respondeu {statusCode}.",
                    data: data);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return new HealthCheckResult(
                context.Registration.FailureStatus,
                $"{_url.Host} inacessível.",
                ex,
                data);
        }
    }
}
