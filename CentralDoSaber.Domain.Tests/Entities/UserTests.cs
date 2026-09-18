using CentralDoSaber.Domain.Entities;
using CentralDoSaber.Domain.Exceptions;

namespace CentralDoSaber.Domain.Tests.Entities;

/// <summary>
/// Regras do usuário: nome obrigatório, e-mail válido, idade mínima de 13 anos
/// e senha com no mínimo 8 caracteres.
/// </summary>
public class UserTests
{
    private static readonly DateOnly Hoje = DateOnly.FromDateTime(DateTime.Today);

    [Fact]
    public void Construtor_DadosValidos_CriaUsuarioAtivoComIdadeCalculada()
    {
        // Arrange
        var dataNascimento = Hoje.AddYears(-20);

        // Act
        var user = new User("Maria Silva", "maria@exemplo.com", dataNascimento, "senhaSegura123");

        // Assert
        Assert.Equal("Maria Silva", user.Nome);
        Assert.Equal("maria@exemplo.com", user.Email);
        Assert.Equal(20, user.Idade);
        Assert.True(user.Disponivel);
        Assert.NotEqual(Guid.Empty, user.Id);
    }

    [Fact]
    public void DefinirDataNascimento_ExatamenteTrezeAnos_AceitaData()
    {
        // Arrange
        var user = new User("Maria", "maria@exemplo.com", Hoje.AddYears(-30), "senhaSegura123");
        var trezeAnosHoje = Hoje.AddYears(-13);

        // Act
        user.DefinirDataNascimento(trezeAnosHoje);

        // Assert
        Assert.Equal(13, user.Idade);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(12)]
    public void DefinirDataNascimento_MenorDeTrezeAnos_LancaDomainException(int idade)
    {
        // Arrange
        var user = new User("Maria", "maria@exemplo.com", Hoje.AddYears(-30), "senhaSegura123");
        var dataNascimento = Hoje.AddYears(-idade);

        // Act
        var act = () => user.DefinirDataNascimento(dataNascimento);

        // Assert
        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("Usuário deve ter pelo menos 13 anos.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("sem-arroba.com")]
    public void AtualizarEmail_EmailInvalido_LancaDomainException(string emailInvalido)
    {
        // Arrange
        var user = new User("Maria", "maria@exemplo.com", Hoje.AddYears(-20), "senhaSegura123");

        // Act
        var act = () => user.AtualizarEmail(emailInvalido);

        // Assert
        Assert.Throws<DomainException>(act);
        Assert.Equal("maria@exemplo.com", user.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1234567")]
    [InlineData("       ")]
    public void AlterarSenha_SenhaCurtaOuVazia_LancaDomainException(string senhaInvalida)
    {
        // Arrange
        var user = new User("Maria", "maria@exemplo.com", Hoje.AddYears(-20), "senhaSegura123");

        // Act
        var act = () => user.AlterarSenha(senhaInvalida);

        // Assert
        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("A senha deve ter pelo menos 8 caracteres.", ex.Message);
    }
}
