using CentralDoSaber.Domain.Commom;

namespace CentralDoSaber.Domain.Entities;

public class Genero : BaseEntity
{
    public string Nome { get; private set; }

    public string Descricao { get; private set; }

    public IReadOnlyCollection<Conteudo> Conteudos { get; private set; } = new List<Conteudo>();

    public Genero(string nome, string descricao)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("Nome do gênero não pode ser vazio.");
        
        if (string.IsNullOrWhiteSpace(descricao))
            throw new Exception("Descrição do gênero não pode ser vazia.");

        Nome = nome;
        Descricao = descricao;
    }
}