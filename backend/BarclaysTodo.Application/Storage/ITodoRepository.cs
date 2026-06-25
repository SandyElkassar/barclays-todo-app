using BarclaysTodo.Domain;

namespace BarclaysTodo.Application.Storage;

public interface ITodoRepository
{
    IReadOnlyCollection<TodoItem> GetAll();
    TodoItem? GetById(Guid id);
    TodoItem Add(TodoItem todo);
    TodoItem? Update(TodoItem todo);
    bool Delete(Guid id);
}