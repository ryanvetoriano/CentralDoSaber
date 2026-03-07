using CentralDoSaber.Domain.Commom;

namespace CentralDoSaber.Domain.Entities;

public class Editora : BaseEntity
{
    public string Nome { get; private set; }

    public string Pais { get; private set; }

    // 1..N
    public List<Conteudo> Conteudos { get; private set; } = new();

    public Editora(string nome, string pais)
    {
        AtualizarNome(nome);
        Pais = pais;
    }

    public void AtualizarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("Nome da editora não pode ser vazio.");

        Nome = nome;
    }
}