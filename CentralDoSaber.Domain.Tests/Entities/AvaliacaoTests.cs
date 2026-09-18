using CentralDoSaber.Domain.Entities;
using CentralDoSaber.Domain.Exceptions;

namespace CentralDoSaber.Domain.Tests.Entities;

/// <summary>
/// Regra do MER: a nota de uma avaliação deve estar entre 1 e 5.
/// </summary>
public class AvaliacaoTests
{
    [Fact]
    public void Construtor_NotaDentroDaFaixa_CriaAvaliacaoComNota()
    {
        // Arrange
        var conteudoId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        const int nota = 4;

        // Act
        var avaliacao = new Avaliacao(conteudoId, userId, nota);

        // Assert
        Assert.Equal(nota, avaliacao.Nota);
        Assert.Equal(conteudoId, avaliacao.ConteudoId);
        Assert.Equal(userId, avaliacao.UserId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void Construtor_NotaNosLimites_AceitaNota(int nota)
    {
        // Arrange
        var conteudoId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var avaliacao = new Avaliacao(conteudoId, userId, nota);

        // Assert
        Assert.Equal(nota, avaliacao.Nota);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    public void Construtor_NotaForaDaFaixa_LancaDomainException(int notaInvalida)
    {
        // Arrange
        var conteudoId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var act = () => new Avaliacao(conteudoId, userId, notaInvalida);

        // Assert
        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("A nota deve estar entre 1 e 5.", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void AtualizarNota_NotaInvalida_LancaDomainExceptionEMantemNotaAnterior(int notaInvalida)
    {
        // Arrange
        var avaliacao = new Avaliacao(Guid.NewGuid(), Guid.NewGuid(), 3);

        // Act
        var act = () => avaliacao.AtualizarNota(notaInvalida);

        // Assert
        Assert.Throws<DomainException>(act);
        Assert.Equal(3, avaliacao.Nota);
    }
}
