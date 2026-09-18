using CentralDoSaber.Application.DTO;

namespace CentralDoSaber.Application.Interfaces;

public interface IGeneroService
{
    Task<IReadOnlyList<GeneroResponse>> ListarTodos();

    /// <exception cref="Domain.Exceptions.NotFoundException">Gênero inexistente.</exception>
    Task<GeneroResponse> BuscarPorId(Guid id);

    /// <exception cref="Domain.Exceptions.ConflictException">Já existe gênero com o mesmo nome.</exception>
    Task<GeneroResponse> Criar(GeneroRequest request);

    /// <exception cref="Domain.Exceptions.NotFoundException">Gênero inexistente.</exception>
    /// <exception cref="Domain.Exceptions.ConflictException">Já existe outro gênero com o mesmo nome.</exception>
    Task<GeneroResponse> Atualizar(Guid id, GeneroRequest request);

    /// <exception cref="Domain.Exceptions.NotFoundException">Gênero inexistente.</exception>
    Task Remover(Guid id);
}
