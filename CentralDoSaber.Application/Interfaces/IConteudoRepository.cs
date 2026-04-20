using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Application.Interfaces;

public interface IConteudoRepository
{
    Task<Conteudo?> GetByIdAsync(Guid id);
    Task<List<Conteudo>> GetAllAsync();
    Task AddAsync(Conteudo conteudo);
    Task UpdateAsync(Conteudo conteudo);
    Task DeleteAsync(Conteudo conteudo);
    Task SaveChangesAsync();
}