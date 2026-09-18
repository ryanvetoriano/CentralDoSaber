using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Domain.Entities;
using CentralDoSaber.Domain.Exceptions;

namespace CentralDoSaber.Application.Services;

/// <summary>
/// CRUD de gêneros usando exclusivamente o repositório genérico <see cref="IRepository{T}"/>.
/// </summary>
public class GeneroService : IGeneroService
{
    private readonly IRepository<Genero> _repository;

    public GeneroService(IRepository<Genero> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<GeneroResponse>> ListarTodos()
    {
        var generos = await _repository.GetAllAsync();
        return generos.Select(GeneroResponse.FromDomain).ToList();
    }

    public async Task<GeneroResponse> BuscarPorId(Guid id)
    {
        var genero = await ObterOuFalhar(id);
        return GeneroResponse.FromDomain(genero);
    }

    public async Task<GeneroResponse> Criar(GeneroRequest request)
    {
        await GarantirNomeDisponivel(request.Nome, excluirId: null);

        var genero = new Genero(request.Nome, request.Descricao);

        await _repository.AddAsync(genero);
        await _repository.SaveChangesAsync();

        return GeneroResponse.FromDomain(genero);
    }

    public async Task<GeneroResponse> Atualizar(Guid id, GeneroRequest request)
    {
        var genero = await ObterOuFalhar(id);

        await GarantirNomeDisponivel(request.Nome, excluirId: id);

        genero.AtualizarNome(request.Nome);
        genero.AtualizarDescricao(request.Descricao);

        _repository.Update(genero);
        await _repository.SaveChangesAsync();

        return GeneroResponse.FromDomain(genero);
    }

    public async Task Remover(Guid id)
    {
        var genero = await ObterOuFalhar(id);

        _repository.Delete(genero);
        await _repository.SaveChangesAsync();
    }

    private async Task<Genero> ObterOuFalhar(Guid id) =>
        await _repository.GetByIdAsync(id)
        ?? throw new NotFoundException($"Gênero {id} não encontrado.");

    private async Task GarantirNomeDisponivel(string nome, Guid? excluirId)
    {
        var nomeNormalizado = nome.Trim().ToUpper();

        var emUso = await _repository.ExistsAsync(g =>
            g.Nome.ToUpper() == nomeNormalizado &&
            (excluirId == null || g.Id != excluirId));

        if (emUso)
            throw new ConflictException($"Já existe um gênero com o nome '{nome.Trim()}'.");
    }
}
