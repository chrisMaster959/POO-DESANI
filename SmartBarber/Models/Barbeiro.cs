public class Barbeiro : Pessoa
{
    public string Logradouro { get; set; } = string.Empty;
    public int Nr_Logradouro { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public int cep { get; set; }
    public List<int> ServicosIds { get; set; } = new List<int>();
}