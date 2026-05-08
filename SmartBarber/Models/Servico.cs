public class Servico
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public double Preco { get; set; }

    public Servico() {}

    public Servico(int id, string nome, double preco)
    {
        Id = id;
        Nome = nome;
        Preco = preco;
    }
}