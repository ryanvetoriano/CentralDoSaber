using CentralDoSaber.Domain.Commom;

namespace CentralDoSaber.Domain.Entities;

public class Genero : BaseEntity
{
    public string Nome { get; private set; }
    
    public string Descricao { get; private set; }
    
    public IReadOnlyCollection<Conteudo> Contents { get; private set; }
    
    public Genero(string nome,  string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}