namespace CentralDoSaber.Domain.Entities;

public class ConteudoGenero
{
    public Guid ConteudoId { get; set; }
    public Conteudo Conteudo { get; set; }

    public Guid GeneroId { get; set; }
    public Genero Genero { get; set; }
}