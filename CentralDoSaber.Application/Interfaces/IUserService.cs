using CentralDoSaber.Application.DTO;

namespace CentralDoSaber.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse> CriarUsuario(CreateUserRequest request);

    Task<UserResponse?> BuscarPorId(Guid id);

    Task<UserResponse?> BuscarPorEmail(string email);

    Task<IReadOnlyList<UserResponse>> ListarTodos();

    Task<UserResponse> AtualizarUsuario(Guid id, UpdateUserRequest request);

    Task<bool> DesativarUsuario(Guid id);

    Task<bool> AtivarUsuario(Guid id);

    Task<bool> EmailExiste(string email, Guid? excluirUserId = null);
}