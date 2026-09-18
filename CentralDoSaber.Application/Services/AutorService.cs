using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Domain.Entities;
using CentralDoSaber.Domain.Exceptions;

namespace CentralDoSaber.Application.Services;

/// <summary>
/// Cadastro de autores usando o repositório genérico <see cref="IRepository{T}"/>.
/// </summary>
public class AutorService : IAutorService
{
    private readonly IRepository<Autor> _repository;

    public AutorService(IRepository<Autor> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<AutorResponse>> ListarTodos()
    {
        var autores = await _repository.GetAllAsync();
        return autores.Select(AutorResponse.FromDomain).ToList();
    }

    public async Task<AutorResponse> BuscarPorId(Guid id)
    {
        var autor = await ObterOuFalhar(id);
        return AutorResponse.FromDomain(autor);
    }

    public async Task<AutorResponse> Criar(AutorRequest request)
    {
        var autor = new Autor(request.Nome, request.Biografia, request.DataNascimento);

        await _repository.AddAsync(autor);
        await _repository.SaveChangesAsync();

        return AutorResponse.FromDomain(autor);
    }

    public async Task<AutorResponse> Atualizar(Guid id, AutorRequest request)
    {
        var autor = await ObterOuFalhar(id);

        autor.AtualizarNome(request.Nome);
        autor.AtualizarBiografia(request.Biografia);
        autor.DefinirDataNascimento(request.DataNascimento);

        _repository.Update(autor);
        await _repository.SaveChangesAsync();

        return AutorResponse.FromDomain(autor);
    }

    private async Task<Autor> ObterOuFalhar(Guid id) =>
        await _repository.GetByIdAsync(id)
        ?? throw new NotFoundException($"Autor {id} não encontrado.");
}
