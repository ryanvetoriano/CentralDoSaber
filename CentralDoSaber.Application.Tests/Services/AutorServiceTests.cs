using CentralDoSaber.Application.DTO;
using CentralDoSaber.Application.Interfaces;
using CentralDoSaber.Application.Services;
using CentralDoSaber.Domain.Entities;
using CentralDoSaber.Domain.Exceptions;
using Moq;

namespace CentralDoSaber.Application.Tests.Services;

/// <summary>
/// Testes do AutorService com mock do repositório genérico IRepository&lt;Autor&gt;.
/// </summary>
public class AutorServiceTests
{
    private readonly Mock<IRepository<Autor>> _repository = new(MockBehavior.Strict);
    private readonly AutorService _sut;

    public AutorServiceTests()
    {
        _sut = new AutorService(_repository.Object);
    }

    [Fact]
    public async Task Atualizar_AutorInexistente_LancaNotFoundExceptionENaoAtualiza()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new AutorRequest("Machado de Assis", "Escritor brasileiro.", null);
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Autor?)null);

        // Act
        var act = () => _sut.Atualizar(id, request);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
        _repository.Verify(r => r.Update(It.IsAny<Autor>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Criar_DataNascimentoFutura_LancaDomainExceptionENaoAdiciona()
    {
        // Arrange
        var amanha = DateOnly.FromDateTime(DateTime.Today).AddDays(1);
        var request = new AutorRequest("Machado de Assis", "Escritor brasileiro.", amanha);

        // Act
        var act = () => _sut.Criar(request);

        // Assert
        await Assert.ThrowsAsync<DomainException>(act);
        _repository.Verify(r => r.AddAsync(It.IsAny<Autor>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Criar_DadosValidos_AdicionaEPersisteUmaVez()
    {
        // Arrange
        var request = new AutorRequest("Machado de Assis", "Escritor brasileiro.", new DateOnly(1839, 6, 21));
        _repository.Setup(r => r.AddAsync(It.IsAny<Autor>())).Returns(Task.CompletedTask);
        _repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var response = await _sut.Criar(request);

        // Assert
        Assert.Equal("Machado de Assis", response.Nome);
        Assert.Equal(new DateOnly(1839, 6, 21), response.DataNascimento);
        _repository.Verify(r => r.AddAsync(It.IsAny<Autor>()), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
