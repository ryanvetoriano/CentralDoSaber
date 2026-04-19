using CentralDoSaber.Domain.Commom;

namespace CentralDoSaber.Domain.Entities;

public class Genero : BaseEntity
{
    public string Nome { get; private set; }

    public string Descricao { get; private set; }

    public List<ConteudoGenero> ConteudoGeneros { get; private set; } = new();
    public Genero(string nome, string descricao)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("Nome do gênero não pode ser vazio.");

        Nome = nome;
        Descricao = descricao;
    }
}