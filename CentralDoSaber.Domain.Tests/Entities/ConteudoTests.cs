using CentralDoSaber.Domain.Entities;
using CentralDoSaber.Domain.Enum;
using CentralDoSaber.Domain.Exceptions;

namespace CentralDoSaber.Domain.Tests.Entities;

/// <summary>
/// Regras do conteúdo: título obrigatório, descrição com 10+ caracteres,
/// ano de lançamento plausível, páginas/capítulos para mídias impressas
/// e ao menos um gênero.
/// </summary>
public class ConteudoTests
{
    private const string DescricaoValida = "Uma descrição com tamanho suficiente.";

    [Fact]
    public void Construtor_LivroComPaginasECapitulos_CriaConteudo()
    {
        // Arrange
        const string titulo = "  Dom Casmurro  ";

        // Act
        var conteudo = new Conteudo(titulo, DescricaoValida, 1899, numeroPaginas: 256, numeroCapitulos: 148, tipo: MediaType.Livro);

        // Assert
        Assert.Equal("Dom Casmurro", conteudo.Titulo);
        Assert.Equal(MediaType.Livro, conteudo.Tipo);
        Assert.Equal(256, conteudo.NumeroPaginas);
        Assert.Equal(148, conteudo.NumeroCapitulos);
    }

    [Theory]
    [InlineData(MediaType.Livro, null, 10)]
    [InlineData(MediaType.Manga, 0, 10)]
    [InlineData(MediaType.HQ, 30, null)]
    [InlineData(MediaType.Revista, 30, 0)]
    public void Construtor_MidiaImpressaSemPaginasOuCapitulos_LancaDomainException(
        MediaType tipo, int? paginas, int? capitulos)
    {
        // Arrange
        const string titulo = "Título";

        // Act
        var act = () => new Conteudo(titulo, DescricaoValida, 2020, paginas, capitulos, tipo);

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(1887)]
    [InlineData(0)]
    [InlineData(9999)]
    public void Construtor_AnoDeLancamentoInvalido_LancaDomainException(int ano)
    {
        // Arrange / Act
        var act = () => new Conteudo("Título", DescricaoValida, ano);

        // Assert
        var ex = Assert.Throws<DomainException>(act);
        Assert.Equal("Ano de lançamento inválido.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("curta")]
    [InlineData("123456789")]
    public void AtualizarDescricao_MenosDeDezCaracteres_LancaDomainException(string descricao)
    {
        // Arrange
        var conteudo = new Conteudo("Título", DescricaoValida, 2020);

        // Act
        var act = () => conteudo.AtualizarDescricao(descricao);

        // Assert
        Assert.Throws<DomainException>(act);
        Assert.Equal(DescricaoValida, conteudo.Descricao);
    }

    [Fact]
    public void AdicionarGeneros_ListaVazia_LancaDomainException()
    {
        // Arrange
        var conteudo = new Conteudo("Título", DescricaoValida, 2020);

        // Act
        var act = () => conteudo.AdicionarGeneros([]);

        // Assert
        Assert.Throws<DomainException>(act);
        Assert.Empty(conteudo.ConteudoGeneros);
    }

    [Fact]
    public void AdicionarGeneros_DoisGeneros_VinculaAmbosAoConteudo()
    {
        // Arrange
        var conteudo = new Conteudo("Título", DescricaoValida, 2020);
        var generos = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        // Act
        conteudo.AdicionarGeneros(generos);

        // Assert
        Assert.Equal(2, conteudo.ConteudoGeneros.Count);
        Assert.All(conteudo.ConteudoGeneros, cg => Assert.Equal(conteudo.Id, cg.ConteudoId));
        Assert.Equal(generos, conteudo.ConteudoGeneros.Select(cg => cg.GeneroId));
    }
}
