using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Application.Interfaces;

public interface IAutorRepository
{
    Task<Autor?> GetByIdAsync(Guid id);
    Task<List<Autor>> GetAllAsync();
    Task AddAsync(Autor autor);
    Task SaveChangesAsync();
}