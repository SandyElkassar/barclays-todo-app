namespace BarclaysTodo.Domain;

public class TodoItem
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public int Priority { get; set; }

    public TodoStatus Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}