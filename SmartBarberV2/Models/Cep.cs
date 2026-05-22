public class Cep
{
    public int Id { get; set; }
    public int Codigo { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int CidadeId { get; set; }
    public Cidade? Cidade { get; set; }
}
