using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Barbeiro : Pessoa
{
    public string? Logradouro { get; set; }

    public int Nr_Logradouro { get; set; }

    public string? Bairro { get; set; }

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    public int CepId { get; set; }

    public Cep? Cep { get; set; }

    public List<Servico> Servicos { get; set; } = new();

    public List<Atendimento> Atendimentos { get; set; } = new();
}