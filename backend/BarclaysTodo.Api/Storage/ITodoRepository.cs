using BarclaysTodo.Api.Models;

namespace BarclaysTodo.Api.Storage;

public interface ITodoRepository
{
    IReadOnlyCollection<TodoItem> GetAll();
    TodoItem? GetById(Guid id);
    TodoItem Add(TodoItem todo);
    TodoItem? Update(TodoItem todo);
    bool Delete(Guid id);
}