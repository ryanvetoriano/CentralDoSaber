using CentralDoSaber.Domain.Common;
using CentralDoSaber.Domain.Enum;

namespace CentralDoSaber.Domain.Entities;

public class Conteudo : BaseEntity
{
    public Guid EditoraId { get; private set; }
    public Editora Editora { get; private set; }

    public Guid AutorId { get; private set; }
    public Autor Autor { get; private set; }

    public string Titulo { get; private set; }
    
    public string Descricao { get; private set; }

    public MediaType? Tipo { get; private set; }

    public int DataLancamento { get; private set; }

    // N..N
    public List<ConteudoGenero> ConteudoGeneros { get; private set; } = new();

    public int? NumeroPaginas { get; private set; } 
    public int? NumeroCapitulos { get; private set; }

    // 1..N
    public List<Avaliacao> Avaliacoes { get; private set; } = new();

    // 1..N
    public List<Comentario> Comentarios { get; private set; } = new();

    private Conteudo() { }

    public Conteudo(
        string titulo,
        string descricao,
        int dataLancamento,
        int? numeroPaginas = null,
        int? numeroCapitulos = null,
        MediaType? tipo = MediaType.Outros)
    {
        AtualizarTitulo(titulo);
        AtualizarDescricao(descricao);

        if (dataLancamento < 1888 || dataLancamento > DateTime.Now.Year + 5)
            throw new Exception("Ano de lançamento inválido.");

        DataLancamento = dataLancamento;

        Tipo = tipo;

        ValidarEspecificacoes(numeroPaginas, numeroCapitulos);

        NumeroPaginas = numeroPaginas;
        NumeroCapitulos = numeroCapitulos;
    }

    public void AdicionarGeneros(List<Guid> generosIds)
    {
        if (generosIds == null || !generosIds.Any())
            throw new Exception("O conteúdo deve ter ao menos um gênero.");

        foreach (var generoId in generosIds)
        {
            ConteudoGeneros.Add(new ConteudoGenero
            {
                ConteudoId = this.Id,
                GeneroId = generoId
            });
        }
    }

    private void ValidarEspecificacoes(int? numeroPaginas, int? numeroCapitulos)
    {
        switch (Tipo)
        {
            case MediaType.Livro:
            case MediaType.Manga:
            case MediaType.HQ:
            case MediaType.Revista:
                ValidarPaginasECapitulos(numeroPaginas, numeroCapitulos, Tipo.ToString());
                break;

            case MediaType.Outros:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ValidarPaginasECapitulos(int? numeroPaginas, int? numeroCapitulos, string tipo)
    {
        if (!numeroPaginas.HasValue || numeroPaginas <= 0)
            throw new Exception($"{tipo}s devem ter uma quantidade de páginas maior que zero.");

        if (!numeroCapitulos.HasValue || numeroCapitulos <= 0)
            throw new Exception($"{tipo}s devem ter ao menos um capítulo.");
    }

    public void AtualizarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new Exception("O título não pode ser vazio.");

        Titulo = titulo.Trim();
    }

    public void AtualizarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao) || descricao.Length < 10)
            throw new Exception("A descrição deve ter pelo menos 10 caracteres.");

        Descricao = descricao;
    }
}