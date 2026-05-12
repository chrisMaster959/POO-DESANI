public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string? Telefone { get; set; }

    public Pessoa() { }

    public Pessoa(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}
