using System;
using System.Collections.Generic;

public class Atendimento
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public int BarbeiroId { get; set; }
    public Cliente? Cliente { get; set; }
    public Barbeiro? Barbeiro { get; set; }
    public List<Servico> Servicos { get; set; } = new();
    public Atendimento() { }

    public Atendimento(int id, DateTime dataHora, string status, int clienteId, int barbeiroId)
    {
        Id = id;
        DataHora = dataHora;
        Status = status;
        ClienteId = clienteId;
        BarbeiroId = barbeiroId;
    }
}
