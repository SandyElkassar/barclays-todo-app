namespace BarclaysTodo.Api.Models;

public record UpdateTodoRequest(
    string? Name,
    int Priority,
    TodoStatus Status
);