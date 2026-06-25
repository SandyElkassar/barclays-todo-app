using BarclaysTodo.Api.Models;
using BarclaysTodo.Api.Services;
using BarclaysTodo.Api.Storage;
using BarclaysTodo.Api.Validations;

namespace BarclaysTodo.Tests;

public sealed class TodoServiceTests
{
    private static TodoService CreateService()
    {
        return new TodoService(
            new InMemoryTodoRepository(),
            new TodoValidator());
    }

    [Fact]
    public void Create_WithValidRequest_CreatesTodo()
    {
        var service = CreateService();

        var result = service.Create(new CreateTodoRequest(
            Name: "Buy milk",
            Priority: 1,
            Status: TodoStatus.NotStarted
        ));

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Buy milk", result.Value.Name);
        Assert.Equal(1, result.Value.Priority);
        Assert.Equal(TodoStatus.NotStarted, result.Value.Status);
    }

    [Fact]
    public void Create_WithEmptyName_ReturnsValidationError()
    {
        var service = CreateService();

        var result = service.Create(new CreateTodoRequest(
            Name: "",
            Priority: 1,
            Status: TodoStatus.NotStarted
        ));

        Assert.False(result.IsSuccess);
        Assert.Contains("Task name is required.", result.Errors);
    }

    [Fact]
    public void Create_WithDuplicateName_ReturnsValidationError()
    {
        var service = CreateService();

        service.Create(new CreateTodoRequest(
            Name: "Buy milk",
            Priority: 1,
            Status: TodoStatus.NotStarted
        ));

        var result = service.Create(new CreateTodoRequest(
            Name: "Buy milk",
            Priority: 2,
            Status: TodoStatus.InProgress
        ));

        Assert.False(result.IsSuccess);
        Assert.Contains("A task with the same name already exists.", result.Errors);
    }

    [Fact]
    public void Update_WithValidRequest_UpdatesTodo()
    {
        var service = CreateService();

        var created = service.Create(new CreateTodoRequest(
            Name: "Buy milk",
            Priority: 1,
            Status: TodoStatus.NotStarted
        ));

        var result = service.Update(created.Value!.Id, new UpdateTodoRequest
        (
            Name: "Buy bread",
            Priority: 2,
            Status: TodoStatus.InProgress
        ));

        Assert.True(result.IsSuccess);
        Assert.Equal("Buy bread", result.Value!.Name);
        Assert.Equal(2, result.Value.Priority);
        Assert.Equal(TodoStatus.InProgress, result.Value.Status);
    }

    [Fact]
    public void Update_WithDuplicateName_ReturnsValidationError()
    {
        var service = CreateService();

        service.Create(new CreateTodoRequest(
            Name: "Buy milk",
            Priority: 1,
            Status: TodoStatus.NotStarted
        ));

        var second = service.Create(new CreateTodoRequest(
            Name: "Clean Room",
            Priority: 2,
            Status: TodoStatus.NotStarted
        ));

        var result = service.Update(second.Value!.Id, new UpdateTodoRequest
        (
            Name: "BUY MILK",
            Priority: 3,
            Status: TodoStatus.InProgress
        ));

        Assert.False(result.IsSuccess);
        Assert.Contains("A task with the same name already exists.", result.Errors);
    }

    [Fact]
    public void Delete_NotCompletedTodo_ReturnsValidationError()
    {
        var service = CreateService();

        var created = service.Create(new CreateTodoRequest(
            Name: "Buy milk",
            Priority: 1,
            Status: TodoStatus.InProgress
        ));

        var result = service.Delete(created.Value!.Id);

        Assert.False(result.IsSuccess);
        Assert.Contains("Only completed tasks can be deleted.", result.Errors);
    }

    [Fact]
    public void Delete_CompletedTodo_DeletesTodo()
    {
        var service = CreateService();

        var created = service.Create(new CreateTodoRequest(
            Name: "Buy milk",
            Priority: 1,
            Status: TodoStatus.Completed
        ));

        var result = service.Delete(created.Value!.Id);

        Assert.True(result.IsSuccess);
        Assert.Null(service.GetById(created.Value.Id));
    }

    [Fact]
    public void Delete_UnknownTodo_ReturnsNotFound()
    {
        var service = CreateService();

        var result = service.Delete(Guid.NewGuid());

        Assert.False(result.IsSuccess);
        Assert.True(result.NotFound);
        Assert.Contains("Task was not found.", result.Errors);
    }
}