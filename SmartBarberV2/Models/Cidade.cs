using System.Collections.Generic;

public class Cidade
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CodigoIBGE { get; set; } = string.Empty;
    public List<Cep> Ceps { get; set; } = new List<Cep>();
}
