namespace BarclaysTodo.Api.Models;

public class TodoItem
{
    public Guid Id { get; init; }
    public required string Name { get; set; }
    public int Priority { get; set; }
    public TodoStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; set; }
}