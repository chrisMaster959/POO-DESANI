public class Pessoa
{
  public int Codigo {get; set;}
  public string Nome {get; set;}

  public Pessoa() {}

  public Pessoa(int codigo,string nome)
  {
    Codigo = codigo;
    Nome = nome;
  }
}