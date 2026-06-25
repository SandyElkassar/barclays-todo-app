using BarclaysTodo.Application.Dtos;
using BarclaysTodo.Application.Services;
using BarclaysTodo.Domain;

namespace BarclaysTodo.Application.Validations;

public class TodoValidator: ITodoValidator
{
    public ValidationResult ValidateForCreate(CreateTodoRequest request, IReadOnlyCollection<TodoItem> existingTodos)
    {
        return Validate(
            request.Name,
            request.Priority,
            request.Status,
            existingTodos);
    }

    public ValidationResult ValidateForUpdate(Guid id, UpdateTodoRequest request, IReadOnlyCollection<TodoItem> existingTodos)
    {
        return Validate(
            request.Name,
            request.Priority,
            request.Status,
            existingTodos,
            excludedTodoId: id);
    }
    
    private static ValidationResult Validate(
        string? name,
        int priority,
        TodoStatus status,
        IReadOnlyCollection<TodoItem> existingTodos,
        Guid? excludedTodoId=null)
    {
        var result = new ValidationResult();

        ValidateName(name, result);
        ValidatePriority(priority, result);
        ValidateStatus(status, result);

        if (HasDuplicateName(name, existingTodos, excludedTodoId))
        {
            result.AddError("A task with the same name already exists.");
        }

        return result;
    }
    
    private static void ValidateName(string? name, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            result.AddError("Task name is required.");
        }
    }

    private static void ValidatePriority(int priority, ValidationResult result)
    {
        if (priority < 0)
        {
            result.AddError("Priority must be a non-negative number.");
        }
    }

    private static void ValidateStatus(TodoStatus status, ValidationResult result)
    {
        if (!Enum.IsDefined(typeof(TodoStatus), status))
        {
            result.AddError("Task status is invalid.");
        }
    }
    
    private static bool HasDuplicateName(
        string? name,
        IReadOnlyCollection<TodoItem> existingTodos,
        Guid? excludedTodoId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        return existingTodos.Any(todo =>
            todo.Id != excludedTodoId &&
            string.Equals(todo.Name.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}