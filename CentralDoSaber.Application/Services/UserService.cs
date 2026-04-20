using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;

namespace CentralDoSaber.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserResponse> CriarUsuario(CreateUserRequest request)
    {
        if (await _repository.EmailExistsAsync(request.Email))
            throw new InvalidOperationException("E-mail já está em uso.");

        var user = request.ToDomain();

        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();

        return UserResponse.FromDomain(user);
    }

    public async Task<UserResponse?> BuscarPorId(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        return user is null ? null : UserResponse.FromDomain(user);
    }

    public async Task<UserResponse?> BuscarPorEmail(string email)
    {
        var user = await _repository.GetByEmailAsync(email);
        return user is null ? null : UserResponse.FromDomain(user);
    }

    public async Task<IReadOnlyList<UserResponse>> ListarTodos()
    {
        var users = await _repository.GetAllAsync();
        return users.Select(UserResponse.FromDomain).ToList();
    }

    public async Task<UserResponse> AtualizarUsuario(Guid id, UpdateUserRequest request)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            throw new InvalidOperationException("Usuário não encontrado.");

        if (request.Email is not null &&
            await _repository.EmailExistsAsync(request.Email, id))
            throw new InvalidOperationException("E-mail já está em uso.");

        if (request.Nome is not null)
            user.AtualizarNome(request.Nome);

        if (request.Email is not null)
            user.AtualizarEmail(request.Email);

        if (request.DataNascimento is not null)
            user.DefinirDataNascimento(request.DataNascimento.Value);

        if (request.NovaSenha is not null)
            user.AlterarSenha(request.NovaSenha);

        await _repository.UpdateAsync(user);
        await _repository.SaveChangesAsync();

        return UserResponse.FromDomain(user);
    }

    public async Task<bool> DesativarUsuario(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return false;

        user.Deactivate();
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AtivarUsuario(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return false;

        user.Activate();
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EmailExiste(string email, Guid? excluirUserId = null)
    {
        return await _repository.EmailExistsAsync(email, excluirUserId);
    }
}