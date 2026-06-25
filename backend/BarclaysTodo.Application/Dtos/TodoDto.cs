using BarclaysTodo.Domain;

namespace BarclaysTodo.Application.Dtos;

public record TodoDto(
    Guid Id,
    string Name,
    int Priority,
    TodoStatus Status,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc)
{
    public static TodoDto FromDomain(TodoItem todo)
    {
        return new TodoDto(
            todo.Id,
            todo.Name,
            todo.Priority,
            todo.Status,
            todo.CreatedAtUtc,
            todo.UpdatedAtUtc);
    }
}