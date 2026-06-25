using BarclaysTodo.Api.Models;

namespace BarclaysTodo.Api.Services;

public interface ITodoService
{
    IReadOnlyCollection<TodoItem> GetAll();
    TodoItem? GetById(Guid id);
    ServiceResult<TodoItem> Create(CreateTodoRequest request);
    ServiceResult<TodoItem> Update(Guid id, UpdateTodoRequest request);
    ServiceResult<TodoItem> Delete(Guid id);
}