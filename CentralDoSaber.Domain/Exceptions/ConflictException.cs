namespace CentralDoSaber.Domain.Exceptions;

/// <summary>
/// Conflito com o estado atual dos dados (ex.: e-mail já cadastrado).
/// Mapeada para HTTP 409 pelo GlobalExceptionHandler.
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}
