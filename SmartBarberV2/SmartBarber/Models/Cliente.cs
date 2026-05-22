using System.Collections.Generic;

public class Cliente : Pessoa
{
    public List<Atendimento> Atendimentos { get; set; } = new List<Atendimento>();

    public Cliente() { }
}