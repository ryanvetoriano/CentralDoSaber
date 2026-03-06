using CentralDoSaber.Domain.Commom;
using CentralDoSaber.Domain.Enum;

namespace CentralDoSaber.Domain.Entities;

public class Conteudo : BaseEntity
{
    public string Titulo { get; private set; }
    
    public string Descricao { get; private set; }

    public MediaType? Tipo { get; private set; }

    public int DataLancamento { get; private set; }
    
    public List<Genero> Generos { get; private set; }

    public int? NumeroPaginas { get; private set; } 
    
    public int? NumeroCapitulos { get; private set; }


    public Conteudo(string titulo, string descricao, int dataLancamento, List<Genero> generos,
        int? numeroPaginas = null, int? numeroCapitulos = null, MediaType? tipo = MediaType.Outros)
    {
        AtualizarTitulo(titulo);
        AtualizarDescricao(descricao);
    
        if (dataLancamento < 1888 || dataLancamento > DateTime.Now.Year + 5)
            throw new Exception("Ano de lançamento inválido.");
        
        DataLancamento = dataLancamento;
    
        if (generos == null || !generos.Any())
            throw new Exception("O conteúdo deve ter ao menos um gênero.");
        
        Generos = generos;

        Tipo = tipo;
    
        ValidarEspecificacoes(numeroPaginas, numeroCapitulos);
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
                if (numeroPaginas <= 0)
                    throw new Exception("Conteúdos devem ter uma quantidade de páginas maior que zero.");
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        NumeroPaginas = numeroPaginas;
        NumeroCapitulos = numeroCapitulos;
    }

    private void ValidarPaginasECapitulos(int? numeroPaginas, int? numeroCapitulos, string tipo)
    {
        if (numeroPaginas <= 0)
            throw new Exception($"{tipo}s devem ter uma quantidade de páginas maior que zero.");

        if (numeroCapitulos <= 0)
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