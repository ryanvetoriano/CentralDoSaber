using CentralDoSaber.Domain.Entities;

namespace CentralDoSaber.Domain.Interfaces;

public interface IUserService
{
    User CriarUsuario(string nome, string email, DateOnly dataNascimento, string senha);

    User BuscarPorEmail(string email);
}