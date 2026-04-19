using CentralDoSaber.Domain.Commom;

namespace CentralDoSaber.Domain.Entities;

public class Comentario : BaseEntity
{
    public string Texto { get; private set; }

    public Guid UserId { get; private set; }
    public Guid ConteudoId { get; private set; }

    public User User { get; private set; }
    public Conteudo Conteudo { get; private set; }

    public Comentario(string texto, Guid userId, Guid conteudoId)
    {
        AtualizarTexto(texto);
        UserId = userId;
        ConteudoId = conteudoId;
    }

    public void AtualizarTexto(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            throw new Exception("Comentário não pode ser vazio.");

        if (texto.Length < 3)
            throw new Exception("Comentário muito curto.");

        Texto = texto;
    }
}