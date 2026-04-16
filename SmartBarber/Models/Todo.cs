public class Todo
{
    // private string id;
    // private string description;
    // private bool done;

    // Métodos Get e Set
    // public string GetId()
    // {
    //     return this.id;
    // }
    // public void SetId(string id)
    // {
    //     this.id = id;
    // }

    // Propriedades
    // public string Id
    // {
    //     get { return id; }
    //     set { id = value; }
    // }

    // Propriedades Automáticas:
    public string Id {get; set; }
    public string Description {get; set; }
    public bool Done { get; set; }

    public Todo()
    {
        Id = "0";
        Done = false;
    }

    public Todo(string id, string description)
    {
        Id = id;
        Description = description;
        Done = false;
    }
    
}