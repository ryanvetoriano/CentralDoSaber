using CentralDoSaber.Application.DTO;

namespace CentralDoSaber.Application.Interfaces;

/// <summary>
/// Interface do serviço de usuários.
/// </summary>
public interface IUserService
{
    UserResponse CriarUsuario(CreateUserRequest request);

    UserResponse? BuscarPorId(Guid id);

    UserResponse? BuscarPorEmail(string email);

    IReadOnlyList<UserResponse> ListarTodos();

    UserResponse AtualizarUsuario(Guid id, UpdateUserRequest request);

    bool DesativarUsuario(Guid id);

    bool AtivarUsuario(Guid id);

    bool EmailExiste(string email, Guid? excluirUserId = null);
}