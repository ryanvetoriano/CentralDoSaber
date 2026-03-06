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
        
        Tipo = tipo;
        
       ValidarEspecificacoes(NumeroPaginas, NumeroCapitulos);
    }

    private void Especificacoes(int? numeroCapitulo, int? numeroCapitulos)
    {
        switch (Tipo)
        {
            case MediaType.Livro:
            {
                if 
            }
        }
    }
    
}