public class Todo
{
    // definir data_hora com sérgio
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