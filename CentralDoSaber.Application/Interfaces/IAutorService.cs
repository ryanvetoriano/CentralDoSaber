using CentralDoSaber.Application.DTO;

namespace CentralDoSaber.Application.Interfaces;

public interface IAutorService
{
    Task<IReadOnlyList<AutorResponse>> ListarTodos();

    /// <exception cref="Domain.Exceptions.NotFoundException">Autor inexistente.</exception>
    Task<AutorResponse> BuscarPorId(Guid id);

    Task<AutorResponse> Criar(AutorRequest request);

    /// <exception cref="Domain.Exceptions.NotFoundException">Autor inexistente.</exception>
    Task<AutorResponse> Atualizar(Guid id, AutorRequest request);
}
