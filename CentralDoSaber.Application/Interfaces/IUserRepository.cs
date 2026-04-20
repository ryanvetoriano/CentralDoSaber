using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();

    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);

    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);

    Task SaveChangesAsync();
}