using BarclaysTodo.Domain;

namespace BarclaysTodo.Application.Storage;

public sealed class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<TodoItem> _todos = new();
    private readonly object _lock = new();

    public IReadOnlyCollection<TodoItem> GetAll()
    {
        lock (_lock)
        {
            return _todos
                .OrderBy(todo => todo.Priority)
                .ThenBy(todo => todo.CreatedAtUtc)
                .ThenBy(todo => todo.Name)
                .Select(Clone)
                .ToList();
        }
    }

    public TodoItem? GetById(Guid id)
    {
        lock (_lock)
        {
            var todo = _todos.Find(item => item.Id == id);
            return todo is null ? null : Clone(todo);
        }
    }

    public TodoItem Add(TodoItem todo)
    {
        lock (_lock)
        {
            _todos.Add(Clone(todo));
            return Clone(todo);
        }
    }

    public TodoItem? Update(TodoItem todo)
    {
        lock (_lock)
        {
            var existing = _todos.FirstOrDefault(item => item.Id == todo.Id);

            if (existing is null)
            {
                return null;
            }

            existing.Name = todo.Name;
            existing.Priority = todo.Priority;
            existing.Status = todo.Status;
            existing.UpdatedAtUtc = todo.UpdatedAtUtc;

            return Clone(existing);
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var existing = _todos.FirstOrDefault(item => item.Id == id);

            if (existing is null)
            {
                return false;
            }

            _todos.Remove(existing);
            return true;
        }
    }

    private static TodoItem Clone(TodoItem todo)
    {
        return new TodoItem
        {
            Id = todo.Id,
            Name = todo.Name,
            Priority = todo.Priority,
            Status = todo.Status,
            CreatedAtUtc = todo.CreatedAtUtc,
            UpdatedAtUtc = todo.UpdatedAtUtc
        };
    }
}