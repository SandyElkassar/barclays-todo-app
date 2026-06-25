using BarclaysTodo.Application.Dtos;
using BarclaysTodo.Application.Services;
using BarclaysTodo.Domain;

namespace BarclaysTodo.Application.Validations;

public interface ITodoValidator
{
    ValidationResult ValidateForCreate(CreateTodoRequest request, IReadOnlyCollection<TodoItem> existingTodos);
    ValidationResult ValidateForUpdate(Guid id, UpdateTodoRequest request, IReadOnlyCollection<TodoItem> existingTodos);
}