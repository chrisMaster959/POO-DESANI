using System.Collections.Generic;

public class Servico
{
    public int Id { get; set; }
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public List<Barbeiro> Barbeiros { get; set; } = new();
    public List<Atendimento>? Atendimentos { get; set; }

    public Servico() { }

    public Servico(int id, string nome, decimal preco)
    {
        Id = id;
        Nome = nome;
        Preco = preco;
    }
}
