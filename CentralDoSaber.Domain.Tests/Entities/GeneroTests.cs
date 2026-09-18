using CentralDoSaber.Domain.Entities;
using CentralDoSaber.Domain.Exceptions;

namespace CentralDoSaber.Domain.Tests.Entities;

/// <summary>
/// Regras do gênero: nome obrigatório (até 100 caracteres) e descrição obrigatória.
/// </summary>
public class GeneroTests
{
    [Fact]
    public void Construtor_DadosValidos_CriaGeneroComNomeSemEspacosExtras()
    {
        // Arrange
        const string nome = "  Fantasia  ";
        const string descricao = "Mundos imaginários e magia.";

        // Act
        var genero = new Genero(nome, descricao);

        // Assert
        Assert.Equal("Fantasia", genero.Nome);
        Assert.Equal(descricao, genero.Descricao);
        Assert.True(genero.Disponivel);
    }

    [Theory]
    [InlineData("", "Descrição válida")]
    [InlineData("   ", "Descrição válida")]
    [InlineData("Fantasia", "")]
    [InlineData("Fantasia", "   ")]
    public void Construtor_NomeOuDescricaoVazios_LancaDomainException(string nome, string descricao)
    {
        // Arrange / Act
        var act = () => new Genero(nome, descricao);

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void AtualizarNome_AcimaDoLimite_LancaDomainExceptionEMantemNome()
    {
        // Arrange
        var genero = new Genero("Fantasia", "Mundos imaginários.");
        var nomeLongo = new string('a', Genero.NomeMaxLength + 1);

        // Act
        var act = () => genero.AtualizarNome(nomeLongo);

        // Assert
        Assert.Throws<DomainException>(act);
        Assert.Equal("Fantasia", genero.Nome);
    }
}
