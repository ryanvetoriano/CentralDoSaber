using CentralDoSaber.Domain.Entities;
using CentralDoSaber.Domain.Exceptions;

namespace CentralDoSaber.Domain.Tests.Entities;

/// <summary>
/// Regras do autor: nome e biografia obrigatórios; data de nascimento não pode ser futura.
/// </summary>
public class AutorTests
{
    private static readonly DateOnly Hoje = DateOnly.FromDateTime(DateTime.Today);

    [Fact]
    public void Construtor_DataNascimentoNoPassado_CriaAutor()
    {
        // Arrange
        var nascimento = new DateOnly(1839, 6, 21);

        // Act
        var autor = new Autor("Machado de Assis", "Escritor brasileiro.", nascimento);

        // Assert
        Assert.Equal("Machado de Assis", autor.Nome);
        Assert.Equal(nascimento, autor.DataNascimento);
    }

    [Fact]
    public void Construtor_SemDataNascimento_CriaAutorComDataNula()
    {
        // Arrange / Act
        var autor = new Autor("Autor Anônimo", "Biografia desconhecida.");

        // Assert
        Assert.Null(autor.DataNascimento);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(30)]
    [InlineData(3650)]
    public void DefinirDataNascimento_DataFutura_LancaDomainException(int diasNoFuturo)
    {
        // Arrange
        var autor = new Autor("Machado de Assis", "Escritor brasileiro.");
        var dataFutura = Hoje.AddDays(diasNoFuturo);

        // Act
        var act = () => autor.DefinirDataNascimento(dataFutura);

        // Assert
        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("Data de nascimento do autor não pode estar no futuro.", ex.Message);
        Assert.Null(autor.DataNascimento);
    }
}
