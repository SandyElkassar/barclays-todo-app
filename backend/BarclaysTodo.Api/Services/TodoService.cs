using BarclaysTodo.Api.Models;
using BarclaysTodo.Api.Storage;
using BarclaysTodo.Api.Validations;

namespace BarclaysTodo.Api.Services;

public sealed class TodoService(ITodoRepository repository, ITodoValidator validator) : ITodoService
{
    public IReadOnlyCollection<TodoItem> GetAll()
    {
        return repository.GetAll();
    }

    public TodoItem? GetById(Guid id)
    {
        return repository.GetById(id);
    }

    public ServiceResult<TodoItem> Create(CreateTodoRequest request)
    {
        var existingTodos = repository.GetAll();
        var validationResult = validator.ValidateForCreate(request, existingTodos);

        if (!validationResult.IsValid)
        {
            return ServiceResult<TodoItem>.Failure(validationResult.Errors);
        }

        var now = DateTime.UtcNow;

        var todo = new TodoItem
        {
            Id = Guid.NewGuid(),
            Name = request.Name!.Trim(),
            Priority = request.Priority,
            Status = request.Status,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        var created = repository.Add(todo);

        return ServiceResult<TodoItem>.Success(created);
    }

    public ServiceResult<TodoItem> Update(Guid id, UpdateTodoRequest request)
    {
        var existingTodo = repository.GetById(id);

        if (existingTodo is null)
        {
            return ServiceResult<TodoItem>.NotFoundResult("Task was not found.");
        }

        var existingTodos = repository.GetAll();
        var validationResult = validator.ValidateForUpdate(id, request, existingTodos);

        if (!validationResult.IsValid)
        {
            return ServiceResult<TodoItem>.Failure(validationResult.Errors);
        }

        existingTodo.Name = request.Name!.Trim();
        existingTodo.Priority = request.Priority;
        existingTodo.Status = request.Status;
        existingTodo.UpdatedAtUtc = DateTime.UtcNow;

        var updated = repository.Update(existingTodo);

        return updated is null
            ? ServiceResult<TodoItem>.NotFoundResult("Task was not found.")
            : ServiceResult<TodoItem>.Success(updated);
    }

    public ServiceResult<TodoItem> Delete(Guid id)
    {
        var existingTodo = repository.GetById(id);

        if (existingTodo is null)
        {
            return ServiceResult<TodoItem>.NotFoundResult("Task was not found.");
        }

        if (existingTodo.Status != TodoStatus.Completed)
        {
            return ServiceResult<TodoItem>.Failure(new[]
            {
                "Only completed tasks can be deleted."
            });
        }

        repository.Delete(id);

        return ServiceResult<TodoItem>.Success(existingTodo);
    }
}