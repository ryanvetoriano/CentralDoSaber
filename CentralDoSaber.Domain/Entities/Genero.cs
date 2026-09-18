using CentralDoSaber.Domain.Common;
using CentralDoSaber.Domain.Exceptions;

namespace CentralDoSaber.Domain.Entities;

public class Genero : BaseEntity
{
    public const int NomeMaxLength = 100;

    public string Nome { get; private set; }

    public string Descricao { get; private set; }

    public List<ConteudoGenero> ConteudoGeneros { get; private set; } = new();

    public Genero(string nome, string descricao)
    {
        AtualizarNome(nome);
        AtualizarDescricao(descricao);
    }

    public void AtualizarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do gênero não pode ser vazio.");

        if (nome.Trim().Length > NomeMaxLength)
            throw new DomainException($"Nome do gênero deve ter no máximo {NomeMaxLength} caracteres.");

        Nome = nome.Trim();
    }

    public void AtualizarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição do gênero não pode ser vazia.");

        Descricao = descricao.Trim();
    }
}
