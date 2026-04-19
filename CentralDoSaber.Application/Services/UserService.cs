using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Application.Services;

public class UserService : IUserService
{
    private readonly List<User> _usuarios = new(); // simulação

    public UserResponse CriarUsuario(CreateUserRequest request)
    {
        if (EmailExiste(request.Email))
            throw new InvalidOperationException("E-mail já está em uso.");

        var user = request.ToDomain();

        _usuarios.Add(user);

        return UserResponse.FromDomain(user);
    }

    public UserResponse? BuscarPorId(Guid id)
    {
        var user = _usuarios.FirstOrDefault(u => u.Id == id);
        return user is null ? null : UserResponse.FromDomain(user);
    }

    public UserResponse? BuscarPorEmail(string email)
    {
        var user = _usuarios.FirstOrDefault(u => u.Email == email);
        return user is null ? null : UserResponse.FromDomain(user);
    }

    public IReadOnlyList<UserResponse> ListarTodos()
    {
        return _usuarios
            .Where(u => u.Disponivel)
            .Select(UserResponse.FromDomain)
            .ToList();
    }

    public UserResponse AtualizarUsuario(Guid id, UpdateUserRequest request)
    {
        var user = _usuarios.FirstOrDefault(u => u.Id == id);

        if (user == null)
            throw new InvalidOperationException("Usuário não encontrado.");

        if (request.Email is not null && EmailExiste(request.Email, id))
            throw new InvalidOperationException("E-mail já está em uso.");

        if (request.Nome is not null)
            user.AtualizarNome(request.Nome);

        if (request.Email is not null)
            user.AtualizarEmail(request.Email);

        if (request.DataNascimento is not null)
            user.DefinirDataNascimento(request.DataNascimento.Value);

        if (request.NovaSenha is not null)
            user.AlterarSenha(request.NovaSenha);

        return UserResponse.FromDomain(user);
    }

    public bool DesativarUsuario(Guid id)
    {
        var user = _usuarios.FirstOrDefault(u => u.Id == id);
        if (user == null) return false;

        user.Deactivate();
        return true;
    }

    public bool AtivarUsuario(Guid id)
    {
        var user = _usuarios.FirstOrDefault(u => u.Id == id);
        if (user == null) return false;

        user.Activate();
        return true;
    }

    public bool EmailExiste(string email, Guid? excluirUserId = null)
    {
        return _usuarios.Any(u =>
            u.Email == email &&
            (!excluirUserId.HasValue || u.Id != excluirUserId));
    }
}