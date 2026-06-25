using BarclaysTodo.Domain;

namespace BarclaysTodo.Application.Dtos;

public record UpdateTodoRequest(
    string? Name,
    int Priority,
    TodoStatus Status
);