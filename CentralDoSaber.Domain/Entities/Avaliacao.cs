using CentralDoSaber.Domain.Commom;

namespace CentralDoSaber.Domain.Entities;

public class Avaliacao : BaseEntity
{
    public Guid ConteudoId { get; private set; }

    public Guid UserId { get; private set; }

    public Conteudo Conteudo { get; private set; }

    public User User { get; private set; }

    public int Nota { get; private set; }

    public Avaliacao(Guid conteudoId, Guid userId, int nota)
    {
        ConteudoId = conteudoId;
        UserId = userId;
        AtualizarNota(nota);
    }

    public void AtualizarNota(int nota)
    {
        if (nota is < 1 or > 5)
            throw new Exception("A nota deve estar entre 1 e 5.");

        Nota = nota;
    }
}