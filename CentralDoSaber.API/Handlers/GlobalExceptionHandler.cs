using CentralDoSaber.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CentralDoSaber.API.Handlers;

/// <summary>
/// Converte exceções não tratadas em respostas ProblemDetails (RFC 7807)
/// e registra cada ocorrência no log com o traceId da requisição.
/// Em Production a resposta nunca contém stack trace — o detalhe fica só no log.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IProblemDetailsService problemDetailsService,
        IHostEnvironment environment)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;
        var (status, title) = Map(exception);

        _logger.LogError(
            exception,
            "Exceção tratada {ExceptionType} em {Method} {Path} -> {StatusCode}. TraceId: {TraceId}",
            exception.GetType().Name,
            httpContext.Request.Method,
            httpContext.Request.Path.Value,
            status,
            traceId);

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            // Mensagens de regra de negócio são seguras para o cliente;
            // erros inesperados (500) não expõem detalhes internos.
            Detail = status == StatusCodes.Status500InternalServerError
                ? "Ocorreu um erro inesperado. Informe o traceId ao suporte."
                : exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        problem.Extensions["traceId"] = traceId;

        if (_environment.IsDevelopment())
        {
            problem.Extensions["exception"] = exception.GetType().FullName;
            problem.Extensions["stackTrace"] = exception.StackTrace;
        }

        httpContext.Response.StatusCode = status;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        });
    }

    private static (int Status, string Title) Map(Exception exception) => exception switch
    {
        DomainException => (StatusCodes.Status400BadRequest, "Regra de negócio violada"),
        NotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
        ConflictException => (StatusCodes.Status409Conflict, "Conflito"),
        KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
        ArgumentException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
        _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor")
    };
}
