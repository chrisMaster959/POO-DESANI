using System.Collections.Generic;

public class Categoria
{
    public int Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public List<Servico> Servicos { get; set; } = new List<Servico>();
}
