using System.Net;
using System.Net.Http.Json;
using BarclaysTodo.Application.Dtos;
using BarclaysTodo.Domain;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BarclaysTodo.Tests;

public sealed class TodoApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TodoApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTodos_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/todos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostTodo_WithValidRequest_ReturnsCreatedTodo()
    {
        var request = new CreateTodoRequest(
            $"Integration task {Guid.NewGuid()}",
            1,
            TodoStatus.NotStarted);

        var response = await _client.PostAsJsonAsync("/api/todos", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var todo = await response.Content.ReadFromJsonAsync<TodoItem>();

        Assert.NotNull(todo);
        Assert.Equal(request.Name, todo.Name);
        Assert.Equal(request.Priority, todo.Priority);
        Assert.Equal(request.Status, todo.Status);
    }

    [Fact]
    public async Task PostTodo_WithDuplicateName_ReturnsBadRequest()
    {
        var name = $"Duplicate integration task {Guid.NewGuid()}";

        var firstRequest = new CreateTodoRequest(
            name,
            1,
            TodoStatus.NotStarted);

        var duplicateRequest = new CreateTodoRequest(
            name.ToLowerInvariant(),
            2,
            TodoStatus.InProgress);

        var firstResponse = await _client.PostAsJsonAsync("/api/todos", firstRequest);
        var duplicateResponse = await _client.PostAsJsonAsync("/api/todos", duplicateRequest);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, duplicateResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteTodo_WhenTodoIsNotCompleted_ReturnsBadRequest()
    {
        var request = new CreateTodoRequest(
            $"Not completed task {Guid.NewGuid()}",
            1,
            TodoStatus.InProgress);

        var createResponse = await _client.PostAsJsonAsync("/api/todos", request);
        var todo = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        Assert.NotNull(todo);

        var deleteResponse = await _client.DeleteAsync($"/api/todos/{todo.Id}");

        Assert.Equal(HttpStatusCode.BadRequest, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteTodo_WhenTodoIsCompleted_ReturnsNoContent()
    {
        var request = new CreateTodoRequest(
            $"Completed task {Guid.NewGuid()}",
            1,
            TodoStatus.Completed);

        var createResponse = await _client.PostAsJsonAsync("/api/todos", request);
        var todo = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        Assert.NotNull(todo);

        var deleteResponse = await _client.DeleteAsync($"/api/todos/{todo.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task PutTodo_WithValidRequest_ReturnsUpdatedTodo()
    {
        var createRequest = new CreateTodoRequest(
            $"Original task {Guid.NewGuid()}",
            1,
            TodoStatus.NotStarted);

        var createResponse = await _client.PostAsJsonAsync("/api/todos", createRequest);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        Assert.NotNull(createdTodo);

        var updateRequest = new UpdateTodoRequest(
            $"Updated task {Guid.NewGuid()}",
            5,
            TodoStatus.Completed);

        var updateResponse = await _client.PutAsJsonAsync($"/api/todos/{createdTodo.Id}", updateRequest);

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedTodo = await updateResponse.Content.ReadFromJsonAsync<TodoItem>();

        Assert.NotNull(updatedTodo);
        Assert.Equal(createdTodo.Id, updatedTodo.Id);
        Assert.Equal(updateRequest.Name, updatedTodo.Name);
        Assert.Equal(updateRequest.Priority, updatedTodo.Priority);
        Assert.Equal(updateRequest.Status, updatedTodo.Status);
    }
}