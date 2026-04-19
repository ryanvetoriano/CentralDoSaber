namespace CentralDoSaber.Application.DTO;

/// <summary>
/// DTO para atualização de usuário.
/// Todos os campos são opcionais.
/// </summary>
public record UpdateUserRequest(
    string? Nome,
    string? Email,
    DateOnly? DataNascimento,
    string? NovaSenha
);