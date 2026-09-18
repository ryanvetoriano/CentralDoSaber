using System.Linq.Expressions;
using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Application.Services;
using CentralDoSaber.Domain.Entities;
using CentralDoSaber.Domain.Exceptions;
using Moq;

namespace CentralDoSaber.Application.Tests.Services;

/// <summary>
/// Testes do GeneroService com mock do repositório genérico IRepository&lt;Genero&gt;.
/// </summary>
public class GeneroServiceTests
{
    private readonly Mock<IRepository<Genero>> _repository = new(MockBehavior.Strict);
    private readonly GeneroService _sut;

    public GeneroServiceTests()
    {
        _sut = new GeneroService(_repository.Object);
    }

    [Fact]
    public async Task Atualizar_GeneroInexistente_LancaNotFoundExceptionENaoAtualiza()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new GeneroRequest("Fantasia", "Mundos imaginários.");
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Genero?)null);

        // Act
        var act = () => _sut.Atualizar(id, request);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
        _repository.Verify(r => r.Update(It.IsAny<Genero>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Remover_GeneroInexistente_LancaNotFoundExceptionENaoRemove()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Genero?)null);

        // Act
        var act = () => _sut.Remover(id);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
        _repository.Verify(r => r.Delete(It.IsAny<Genero>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Criar_NomeJaExistente_LancaConflictExceptionENaoAdiciona()
    {
        // Arrange
        var request = new GeneroRequest("Fantasia", "Mundos imaginários.");
        _repository
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Genero, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var act = () => _sut.Criar(request);

        // Assert
        await Assert.ThrowsAsync<ConflictException>(act);
        _repository.Verify(r => r.AddAsync(It.IsAny<Genero>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Criar_DadosValidos_AdicionaEPersisteUmaVez()
    {
        // Arrange
        var request = new GeneroRequest("  Fantasia  ", "Mundos imaginários.");
        _repository
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Genero, bool>>>()))
            .ReturnsAsync(false);
        _repository.Setup(r => r.AddAsync(It.IsAny<Genero>())).Returns(Task.CompletedTask);
        _repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var response = await _sut.Criar(request);

        // Assert
        Assert.Equal("Fantasia", response.Nome);
        _repository.Verify(r => r.AddAsync(It.Is<Genero>(g => g.Nome == "Fantasia")), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Atualizar_GeneroExistente_AtualizaEPersisteUmaVez()
    {
        // Arrange
        var genero = new Genero("Fantasia", "Mundos imaginários.");
        var request = new GeneroRequest("Fantasia Épica", "Grandes jornadas e magia.");
        _repository.Setup(r => r.GetByIdAsync(genero.Id)).ReturnsAsync(genero);
        _repository
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Genero, bool>>>()))
            .ReturnsAsync(false);
        _repository.Setup(r => r.Update(genero));
        _repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var response = await _sut.Atualizar(genero.Id, request);

        // Assert
        Assert.Equal("Fantasia Épica", response.Nome);
        Assert.Equal("Grandes jornadas e magia.", response.Descricao);
        _repository.Verify(r => r.Update(genero), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
