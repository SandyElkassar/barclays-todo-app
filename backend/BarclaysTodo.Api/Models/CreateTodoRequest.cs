namespace BarclaysTodo.Api.Models;

public record CreateTodoRequest(
    string? Name,
    int Priority,
    TodoStatus Status);