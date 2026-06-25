using BarclaysTodo.Api.Endpoints;
using BarclaysTodo.Api.Services;
using BarclaysTodo.Api.Storage;
using BarclaysTodo.Api.Validations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
builder.Services.AddSingleton<ITodoValidator, TodoValidator>();
builder.Services.AddSingleton<ITodoService, TodoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("Frontend");

app.MapTodoEndpoints();

app.Run();

public partial class Program
{
}