using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CentralDoSaber.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CentralDoSaberContext _context;

    public UserRepository(CentralDoSaberContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await Task.CompletedTask;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Avaliacoes)
            .Include(u => u.Comentarios)
            .ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await Task.CompletedTask;
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
    {
        return await _context.Users.AnyAsync(u =>
            u.Email == email &&
            (!excludeId.HasValue || u.Id != excludeId));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}