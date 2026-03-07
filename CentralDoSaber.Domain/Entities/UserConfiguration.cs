using CentralDoSaber.Domain.Commom;

namespace CentralDoSaber.Domain.Entities;

public class UserConfiguration : BaseEntity
{
    public string Tema { get; private set; }

    public bool NotificacoesAtivas { get; private set; }

    //1..1
    public Guid UserId { get; private set; }

    public UserConfiguration(string tema, bool notificacoesAtivas, Guid userId)
    {
        AtualizarTema(tema);
        NotificacoesAtivas = notificacoesAtivas;
        UserId = userId;
    }

    public void AtualizarTema(string novoTema)
    {
        if (string.IsNullOrWhiteSpace(novoTema))
            throw new Exception("Tema inválido.");

        Tema = novoTema;
    }

    public void AlterarNotificacoes(bool status)
    {
        NotificacoesAtivas = status;
    }
}