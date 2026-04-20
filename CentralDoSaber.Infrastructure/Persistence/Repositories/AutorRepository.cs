using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CentralDoSaber.Infrastructure.Persistence.Repositories;

public class AutorRepository : IAutorRepository
{
    private readonly CentralDoSaberContext _context;

    public AutorRepository(CentralDoSaberContext context)
    {
        _context = context;
    }

    public async Task<Autor?> GetByIdAsync(Guid id) =>
        await _context.Autores.FirstOrDefaultAsync(a => a.Id == id);

    public async Task<List<Autor>> GetAllAsync() =>
        await _context.Autores.Include(a => a.Conteudos).ToListAsync();

    public async Task AddAsync(Autor autor) =>
        await _context.Autores.AddAsync(autor);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}