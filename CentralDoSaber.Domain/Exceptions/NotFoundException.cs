namespace CentralDoSaber.Domain.Exceptions;

/// <summary>
/// Recurso (ou dependência) inexistente.
/// Mapeada para HTTP 404 pelo GlobalExceptionHandler.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
