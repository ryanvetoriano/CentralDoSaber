using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Application.DTO;

/// <summary>
/// DTO para criação de usuário.
/// </summary>
public record CreateUserRequest(
    string Nome,
    string Email,
    string Senha,
    DateOnly DataNascimento
)
{
    public User ToDomain()
    {
        return new User(Nome, Email, DataNascimento, Senha);
    }
}