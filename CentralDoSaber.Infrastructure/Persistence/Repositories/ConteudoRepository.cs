using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CentralDoSaber.Infrastructure.Persistence.Repositories;

public class ConteudoRepository : IConteudoRepository
{
    private readonly CentralDoSaberContext _context;

    public ConteudoRepository(CentralDoSaberContext context)
    {
        _context = context;
    }

    public async Task<Conteudo?> GetByIdAsync(Guid id) =>
        await _context.Conteudos
            .Include(c => c.Autor)
            .Include(c => c.Editora)
            .Include(c => c.ConteudoGeneros).ThenInclude(cg => cg.Genero)
            .Include(c => c.Avaliacoes)
            .Include(c => c.Comentarios)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<Conteudo>> GetAllAsync() =>
        await _context.Conteudos
            .Include(c => c.Autor)
            .Include(c => c.Editora)
            .Include(c => c.ConteudoGeneros).ThenInclude(cg => cg.Genero)
            .ToListAsync();

    public async Task AddAsync(Conteudo conteudo) =>
        await _context.Conteudos.AddAsync(conteudo);

    public async Task UpdateAsync(Conteudo conteudo) =>
        _context.Conteudos.Update(conteudo);

    public async Task DeleteAsync(Conteudo conteudo) =>
        _context.Conteudos.Remove(conteudo);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}