using BarclaysTodo.Application.Dtos;
using BarclaysTodo.Application.Storage;
using BarclaysTodo.Application.Validations;
using BarclaysTodo.Domain;

namespace BarclaysTodo.Application.Services;

public sealed class TodoService(ITodoRepository repository, ITodoValidator validator) : ITodoService
{
    public IReadOnlyCollection<TodoDto> GetAll()
    {
        return repository
            .GetAll()
            .Select(TodoDto.FromDomain)
            .ToList();
    }

    public ServiceResult<TodoDto> GetById(Guid id)
    {
        var todo = repository.GetById(id);

        return todo is null
            ? ServiceResult<TodoDto>.NotFoundResult("Task was not found.")
            : ServiceResult<TodoDto>.Success(TodoDto.FromDomain(todo));
    }

    public ServiceResult<TodoDto> Create(CreateTodoRequest request)
    {
        var existingTodos = repository.GetAll();
        var validationResult = validator.ValidateForCreate(request, existingTodos);

        if (!validationResult.IsValid)
        {
            return ServiceResult<TodoDto>.Failure(validationResult.Errors);
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

        return ServiceResult<TodoDto>.Success(TodoDto.FromDomain(created));
    }

    public ServiceResult<TodoDto> Update(Guid id, UpdateTodoRequest request)
    {
        var existingTodo = repository.GetById(id);

        if (existingTodo is null)
        {
            return ServiceResult<TodoDto>.NotFoundResult("Task was not found.");
        }

        var existingTodos = repository.GetAll();
        var validationResult = validator.ValidateForUpdate(id, request, existingTodos);

        if (!validationResult.IsValid)
        {
            return ServiceResult<TodoDto>.Failure(validationResult.Errors);
        }

        existingTodo.Name = request.Name!.Trim();
        existingTodo.Priority = request.Priority;
        existingTodo.Status = request.Status;
        existingTodo.UpdatedAtUtc = DateTime.UtcNow;

        var updated = repository.Update(existingTodo);

        return updated is null
            ? ServiceResult<TodoDto>.NotFoundResult("Task was not found.")
            : ServiceResult<TodoDto>.Success(TodoDto.FromDomain(updated));
    }

    public ServiceResult Delete(Guid id)
    {
        var existingTodo = repository.GetById(id);

        if (existingTodo is null)
        {
            return ServiceResult.NotFoundResult("Task was not found.");
        }

        if (existingTodo.Status != TodoStatus.Completed)
        {
            return ServiceResult.Failure(new[]
            {
                "Only completed tasks can be deleted."
            });
        }

        repository.Delete(id);

        return ServiceResult.Success();
    }
}