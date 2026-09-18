using System.ComponentModel.DataAnnotations;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Application.DTO;

/// <summary>
/// DTO para criação de usuário.
/// </summary>
public record CreateUserRequest(
    [Required] string Nome,
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string Senha,
    [Required] DateOnly DataNascimento
)
{
    public User ToDomain()
    {
        return new User(Nome, Email, DataNascimento, Senha);
    }
}