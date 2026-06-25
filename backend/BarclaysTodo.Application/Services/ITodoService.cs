using BarclaysTodo.Application.Dtos;

namespace BarclaysTodo.Application.Services;

public interface ITodoService
{
    IReadOnlyCollection<TodoDto> GetAll();

    ServiceResult<TodoDto> GetById(Guid id);

    ServiceResult<TodoDto> Create(CreateTodoRequest request);

    ServiceResult<TodoDto> Update(Guid id, UpdateTodoRequest request);

    ServiceResult Delete(Guid id);
}