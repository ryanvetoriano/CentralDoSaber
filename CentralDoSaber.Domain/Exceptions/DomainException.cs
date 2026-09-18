namespace CentralDoSaber.Domain.Exceptions;

/// <summary>
/// Violação de uma regra de negócio do domínio (invariante de entidade).
/// Mapeada para HTTP 400 pelo GlobalExceptionHandler.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
