namespace Employee.Models;

public class Todo
{
    public long Id { get; set; }
    public string Name { get; set; } 
    public bool IsComplete { get; set; }

    public static Todo Create(string name, bool isComplete)
    {
        var todo = new Todo()
        {
            Name = name,
            IsComplete = isComplete
        };
        return todo;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateIsComplete(bool isComplete)
    {
        IsComplete = isComplete;
    }
}