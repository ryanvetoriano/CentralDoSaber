using CentralDoSaber.Domain.Common;
using CentralDoSaber.Domain.Exceptions;

namespace CentralDoSaber.Domain.Entities;

public class Autor : BaseEntity
{
    public string Nome { get; private set; }

    public string Biografia { get; private set; }

    public DateOnly? DataNascimento { get; private set; }

    // 1..N
    public List<Conteudo> Conteudos { get; private set; } = new();

    public Autor(string nome, string biografia, DateOnly? dataNascimento = null)
    {
        AtualizarNome(nome);
        AtualizarBiografia(biografia);
        DefinirDataNascimento(dataNascimento);
    }

    public void DefinirDataNascimento(DateOnly? dataNascimento)
    {
        if (dataNascimento > DateOnly.FromDateTime(DateTime.Today))
            throw new DomainException("Data de nascimento do autor não pode estar no futuro.");

        DataNascimento = dataNascimento;
    }

    public void AtualizarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do autor não pode ser vazio.");

        Nome = nome;
    }

    public void AtualizarBiografia(string biografia)
    {
        if (string.IsNullOrWhiteSpace(biografia))
            throw new DomainException("Biografia não pode ser vazia.");

        Biografia = biografia;
    }
}