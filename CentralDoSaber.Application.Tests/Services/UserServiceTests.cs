using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Application.Services;
using CentralDoSaber.Domain.Entities;
using CentralDoSaber.Domain.Exceptions;
using Moq;

namespace CentralDoSaber.Application.Tests.Services;

/// <summary>
/// Testes do UserService com o repositório mockado:
/// não sobe API nem banco.
/// </summary>
public class UserServiceTests
{
    private static readonly DateOnly DataNascimentoValida =
        DateOnly.FromDateTime(DateTime.Today).AddYears(-25);

    private readonly Mock<IUserRepository> _repository = new(MockBehavior.Strict);
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _sut = new UserService(_repository.Object);
    }

    // ---------- AtualizarUsuario ----------

    [Fact]
    public async Task AtualizarUsuario_UsuarioInexistente_LancaNotFoundExceptionENaoPersiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new UpdateUserRequest("Novo Nome", null, null, null);
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

        // Act
        var act = () => _sut.AtualizarUsuario(id, request);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
        _repository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task AtualizarUsuario_EmailJaUsadoPorOutro_LancaConflictExceptionENaoPersiste()
    {
        // Arrange
        var user = new User("Maria", "maria@exemplo.com", DataNascimentoValida, "senhaSegura123");
        var request = new UpdateUserRequest(null, "joao@exemplo.com", null, null);
        _repository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _repository.Setup(r => r.EmailExistsAsync("joao@exemplo.com", user.Id)).ReturnsAsync(true);

        // Act
        var act = () => _sut.AtualizarUsuario(user.Id, request);

        // Assert
        await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal("maria@exemplo.com", user.Email);
        _repository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task AtualizarUsuario_DadosValidos_AtualizaEPersisteUmaVez()
    {
        // Arrange
        var user = new User("Maria", "maria@exemplo.com", DataNascimentoValida, "senhaSegura123");
        var request = new UpdateUserRequest("Maria Souza", "maria.souza@exemplo.com", null, null);
        _repository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _repository.Setup(r => r.EmailExistsAsync("maria.souza@exemplo.com", user.Id)).ReturnsAsync(false);
        _repository.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);
        _repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var response = await _sut.AtualizarUsuario(user.Id, request);

        // Assert
        Assert.Equal("Maria Souza", response.Nome);
        Assert.Equal("maria.souza@exemplo.com", response.Email);
        _repository.Verify(r => r.UpdateAsync(user), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    // ---------- CriarUsuario ----------

    [Fact]
    public async Task CriarUsuario_EmailJaCadastrado_LancaConflictExceptionENaoAdiciona()
    {
        // Arrange
        var request = new CreateUserRequest("Maria", "maria@exemplo.com", "senhaSegura123", DataNascimentoValida);
        _repository.Setup(r => r.EmailExistsAsync(request.Email, null)).ReturnsAsync(true);

        // Act
        var act = () => _sut.CriarUsuario(request);

        // Assert
        await Assert.ThrowsAsync<ConflictException>(act);
        _repository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Theory]
    [InlineData("", "maria@exemplo.com", "senhaSegura123")]
    [InlineData("Maria", "email-invalido", "senhaSegura123")]
    [InlineData("Maria", "maria@exemplo.com", "curta")]
    public async Task CriarUsuario_DadosQueViolamODominio_LancaDomainExceptionENaoAdiciona(
        string nome, string email, string senha)
    {
        // Arrange
        var request = new CreateUserRequest(nome, email, senha, DataNascimentoValida);
        _repository.Setup(r => r.EmailExistsAsync(email, null)).ReturnsAsync(false);

        // Act
        var act = () => _sut.CriarUsuario(request);

        // Assert
        await Assert.ThrowsAsync<DomainException>(act);
        _repository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CriarUsuario_DadosValidos_AdicionaEPersisteUmaVez()
    {
        // Arrange
        var request = new CreateUserRequest("Maria", "maria@exemplo.com", "senhaSegura123", DataNascimentoValida);
        _repository.Setup(r => r.EmailExistsAsync(request.Email, null)).ReturnsAsync(false);
        _repository.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var response = await _sut.CriarUsuario(request);

        // Assert
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal("maria@exemplo.com", response.Email);
        Assert.Equal(25, response.Idade);
        _repository.Verify(r => r.AddAsync(It.Is<User>(u => u.Email == request.Email)), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
