using System.Linq.Expressions;
using CentralDoSaber.Domain.Common;

namespace CentralDoSaber.Application.Interfaces;

/// <summary>
/// Contrato genérico de acesso a dados para qualquer entidade do domínio.
/// Implementado na Infrastructure por <c>Repository&lt;T&gt;</c> (EF Core).
/// Repositórios específicos (ex.: <see cref="IUserRepository"/>) continuam
/// existindo quando o agregado precisa de consultas além do CRUD.
/// </summary>
/// <typeparam name="T">Entidade derivada de <see cref="BaseEntity"/>.</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>Lista todas as entidades (leitura sem rastreamento).</summary>
    Task<IReadOnlyList<T>> GetAllAsync();

    /// <summary>Busca por Id (rastreada, pronta para alteração) ou <c>null</c>.</summary>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>Indica se existe uma entidade com o Id informado.</summary>
    Task<bool> ExistsByIdAsync(Guid id);

    /// <summary>Indica se existe ao menos uma entidade que satisfaça o filtro.</summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>Marca a entidade para inclusão.</summary>
    Task AddAsync(T entity);

    /// <summary>Marca a entidade para atualização.</summary>
    void Update(T entity);

    /// <summary>Marca a entidade para exclusão.</summary>
    void Delete(T entity);

    /// <summary>Persiste as alterações pendentes (unidade de trabalho).</summary>
    Task SaveChangesAsync();
}
