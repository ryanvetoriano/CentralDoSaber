using System.ComponentModel.DataAnnotations;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Application.DTO;

/// <summary>Dados para criar ou atualizar um gênero.</summary>
/// <param name="Nome">Nome do gênero (ex.: "Fantasia"). Máximo de 100 caracteres.</param>
/// <param name="Descricao">Descrição do gênero.</param>
public record GeneroRequest(
    [Required, StringLength(Genero.NomeMaxLength)] string Nome,
    [Required] string Descricao
);

/// <summary>Representação pública de um gênero.</summary>
/// <param name="Id">Identificador único.</param>
/// <param name="Nome">Nome do gênero.</param>
/// <param name="Descricao">Descrição do gênero.</param>
/// <param name="Disponivel">Indica se o gênero está ativo.</param>
public record GeneroResponse(Guid Id, string Nome, string Descricao, bool Disponivel)
{
    public static GeneroResponse FromDomain(Genero genero) =>
        new(genero.Id, genero.Nome, genero.Descricao, genero.Disponivel);
}
