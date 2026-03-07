using CentralDoSaber.Domain.Commom;

namespace CentralDoSaber.Domain.Entities;

public class User : BaseEntity
{
    public string Nome { get; private set; }

    public string Email { get; private set; }

    private DateOnly DataNascimento { get; set; }

    public string Password { get; private set; }

    //1..1
    public UserConfiguration Configuration { get; set; }

    //1..N
    public List<Avaliacao> Avaliacoes { get; set; } = new();

    public User(string nome, string email, DateOnly dataNascimento, string rawPassword)
    {
        AtualizarNome(nome);
        AtualizarEmail(email);
        DefinirDataNascimento(dataNascimento);
        AlterarSenha(rawPassword);
    }

    public void AtualizarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new Exception("Nome não pode ser vazio.");

        Nome = novoNome;
    }

    public void AtualizarEmail(string novoEmail)
    {
        if (string.IsNullOrWhiteSpace(novoEmail) || !novoEmail.Contains("@"))
            throw new Exception("E-mail inválido.");

        Email = novoEmail;
    }

    public void DefinirDataNascimento(DateOnly novaData)
    {
        var idade = CalcularIdade(novaData);

        if (idade < 13)
            throw new Exception("Usuário deve ter pelo menos 13 anos.");

        DataNascimento = novaData;
    }

    public void AlterarSenha(string novaSenha)
    {
        if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 8)
            throw new Exception("A senha deve ter pelo menos 8 caracteres.");

        Password = novaSenha;
    }

    public int Idade => CalcularIdade(DataNascimento);

    private static int CalcularIdade(DateOnly data)
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var idade = hoje.Year - data.Year;

        if (data > hoje.AddYears(-idade))
            idade--;

        return idade;
    }
}