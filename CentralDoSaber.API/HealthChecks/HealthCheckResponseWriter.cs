using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CentralDoSaber.API.HealthChecks;

/// <summary>
/// Serializa o <see cref="HealthReport"/> em JSON legível por humanos e por
/// ferramentas de monitoramento. Mensagem de exceção só em Development.
/// </summary>
public static class HealthCheckResponseWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        // Mantém acentos legíveis ("execução" em vez de "execução")
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static Task WriteAsync(HttpContext context, HealthReport report)
    {
        var isDevelopment = context.RequestServices
            .GetRequiredService<IHostEnvironment>()
            .IsDevelopment();

        var payload = new
        {
            status = report.Status.ToString(),
            totalDurationMs = report.TotalDuration.TotalMilliseconds,
            timestamp = DateTimeOffset.UtcNow,
            traceId = context.TraceIdentifier,
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                durationMs = entry.Value.Duration.TotalMilliseconds,
                description = entry.Value.Description,
                tags = entry.Value.Tags,
                data = entry.Value.Data.Count > 0 ? entry.Value.Data : null,
                exception = isDevelopment ? entry.Value.Exception?.Message : null
            })
        };

        context.Response.ContentType = "application/json; charset=utf-8";
        return context.Response.WriteAsync(JsonSerializer.Serialize(payload, JsonOptions));
    }
}
