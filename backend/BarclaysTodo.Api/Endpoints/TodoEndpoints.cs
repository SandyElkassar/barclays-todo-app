using BarclaysTodo.Application.Dtos;
using BarclaysTodo.Application.Services;

namespace BarclaysTodo.Api.Endpoints;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/todos")
            .WithTags("Todos");

        group.MapGet("/", (ITodoService todoService) =>
        {
            var todos = todoService.GetAll();
            return Results.Ok(todos);
        });

        group.MapGet("/{id:guid}", (Guid id, ITodoService todoService) =>
        {
            var todo = todoService.GetById(id);

            return todo is null
                ? Results.NotFound(new[] { "Task was not found." })
                : Results.Ok(todo);
        });

        group.MapPost("/", (CreateTodoRequest request, ITodoService todoService) =>
        {
            var result = todoService.Create(request);

            return result.IsSuccess
                ? Results.Created($"/api/todos/{result.Value!.Id}", result.Value)
                : Results.BadRequest(result.Errors);
        });

        group.MapPut("/{id:guid}", (Guid id, UpdateTodoRequest request, ITodoService todoService) =>
        {
            var result = todoService.Update(id, request);

            if (result.NotFound)
            {
                return Results.NotFound(result.Errors);
            }

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Errors);
        });

        group.MapDelete("/{id:guid}", (Guid id, ITodoService todoService) =>
        {
            var result = todoService.Delete(id);

            if (result.NotFound)
            {
                return Results.NotFound(result.Errors);
            }

            return result.IsSuccess
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        });

        return app;
    }
}