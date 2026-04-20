using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Application.Interfaces;

public interface IGeneroRepository
{
    Task<Genero?> GetByIdAsync(Guid id);
    Task<List<Genero>> GetAllAsync();
    Task AddAsync(Genero genero);
    Task SaveChangesAsync();
}