using BarclaysTodo.Api.Models;

namespace BarclaysTodo.Api.Validations;

public interface ITodoValidator
{
    ValidationResult ValidateForCreate(CreateTodoRequest request, IReadOnlyCollection<TodoItem> existingTodos);
    ValidationResult ValidateForUpdate(Guid id, UpdateTodoRequest request, IReadOnlyCollection<TodoItem> existingTodos);
}