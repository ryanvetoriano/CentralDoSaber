using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CentralDoSaber.Infrastructure.Persistence.Repositories;

public class GeneroRepository : IGeneroRepository
{
    private readonly CentralDoSaberContext _context;

    public GeneroRepository(CentralDoSaberContext context)
    {
        _context = context;
    }

    public async Task<Genero?> GetByIdAsync(Guid id) =>
        await _context.Generos.FirstOrDefaultAsync(g => g.Id == id);

    public async Task<List<Genero>> GetAllAsync() =>
        await _context.Generos.ToListAsync();

    public async Task AddAsync(Genero genero) =>
        await _context.Generos.AddAsync(genero);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}