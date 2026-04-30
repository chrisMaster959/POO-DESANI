public class Pessoa
{
  public int Id {get; set;}
  public string Nome {get; set;}

  public Pessoa() {}

  public Pessoa(int id,string nome)
  {
    Id = id;
    Nome = nome;
  }
}