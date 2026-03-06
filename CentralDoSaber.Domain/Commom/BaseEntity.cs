namespace CentralDoSaber.Domain.Commom;

public abstract class BaseEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public bool Disponivel { get; private set; } = true;
    
    public DateTime DataCriacao { get; private set; } = DateTime.Now;
    
    public void Deactivate() => Disponivel = false;
    
    public void Activate() => Disponivel = true;

}