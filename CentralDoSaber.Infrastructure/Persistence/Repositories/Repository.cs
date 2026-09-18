using System.Linq.Expressions;
using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CentralDoSaber.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementação EF Core de <see cref="IRepository{T}"/> usando <c>Set&lt;T&gt;()</c>.
/// Registrada na DI como open generic:
/// <c>AddScoped(typeof(IRepository&lt;&gt;), typeof(Repository&lt;&gt;))</c>.
/// </summary>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly CentralDoSaberContext _context;
    private readonly DbSet<T> _set;

    public Repository(CentralDoSaberContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync() =>
        await _set.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _set.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<bool> ExistsByIdAsync(Guid id) =>
        await _set.AsNoTracking().AnyAsync(e => e.Id == id);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) =>
        await _set.AsNoTracking().AnyAsync(predicate);

    public async Task AddAsync(T entity) =>
        await _set.AddAsync(entity);

    public void Update(T entity) =>
        _set.Update(entity);

    public void Delete(T entity) =>
        _set.Remove(entity);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
