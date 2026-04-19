using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Application.DTO;

/// <summary>
/// DTO de resposta para dados do usuário.
/// Não expõe senha.
/// </summary>
/// <param name="Id">Identificador único.</param>
/// <param name="Nome">Nome do usuário.</param>
/// <param name="Email">E-mail.</param>
/// <param name="Idade">Idade calculada.</param>
/// <param name="DataCriacao">Data de criação do registro.</param>
public record UserResponse(
    Guid Id,
    string Nome,
    string Email,
    int Idade,
    DateTime DataCriacao
)
{
    /// <summary>Converte a entidade User para o DTO de resposta.</summary>
    public static UserResponse FromDomain(User user)
    {
        return new UserResponse(
            user.Id,
            user.Nome,
            user.Email,
            user.Idade,
            user.DataCriacao
        );
    }
}