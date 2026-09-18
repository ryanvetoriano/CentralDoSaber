using System.ComponentModel.DataAnnotations;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Application.DTO;

/// <summary>Dados para criar ou atualizar um autor.</summary>
/// <param name="Nome">Nome do autor. Máximo de 150 caracteres.</param>
/// <param name="Biografia">Biografia resumida.</param>
/// <param name="DataNascimento">Data de nascimento (opcional, não pode ser futura).</param>
public record AutorRequest(
    [Required, StringLength(150)] string Nome,
    [Required] string Biografia,
    DateOnly? DataNascimento
);

/// <summary>Representação pública de um autor.</summary>
/// <param name="Id">Identificador único.</param>
/// <param name="Nome">Nome do autor.</param>
/// <param name="Biografia">Biografia resumida.</param>
/// <param name="DataNascimento">Data de nascimento, se conhecida.</param>
/// <param name="Disponivel">Indica se o autor está ativo.</param>
/// <param name="DataCriacao">Data de criação do registro.</param>
public record AutorResponse(
    Guid Id,
    string Nome,
    string Biografia,
    DateOnly? DataNascimento,
    bool Disponivel,
    DateTime DataCriacao)
{
    public static AutorResponse FromDomain(Autor autor) =>
        new(autor.Id, autor.Nome, autor.Biografia, autor.DataNascimento, autor.Disponivel, autor.DataCriacao);
}
