using BarclaysTodo.Domain;

namespace BarclaysTodo.Application.Dtos;

public record CreateTodoRequest(
    string? Name,
    int Priority,
    TodoStatus Status);