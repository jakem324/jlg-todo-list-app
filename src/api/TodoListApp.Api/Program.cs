using TodoListApp.Infrastructure.InMemoryDB;
using TodoListApp.Domain;
using TodoListApp.Domain.Commands;
using TodoListApp.Domain.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDevPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.RegisterInMemoryDb();
builder.Services.RegisterDomainServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("LocalDevPolicy");

app.UseHttpsRedirection();

app.MapPost("/initialize", async (TodoListCommands commandHandler) =>
{
    var uuid = await commandHandler.InitializeNewTodoList();
    return Results.Ok(uuid);
});

app.MapPost("/{listId:guid}/add", async (TodoListCommands commandHandler, Guid listId) =>
{
    var (result, createdItemId) = await commandHandler.InitializeNewItem(listId);
    if (result == TodoListCommandResult.ListNotFound)
        return Results.NotFound();

    return Results.Ok(createdItemId);
});

app.MapPost("/{listId:guid}/{itemId:guid}/update", async (TodoListCommands commandHandler, Guid listId, Guid itemId, UpdateListItemDto dto) =>
{
    var result = await commandHandler.UpdateItem(listId, itemId, dto.title, dto.body);
    if (result == TodoListCommandResult.ListNotFound || result == TodoListCommandResult.ItemNotFound)
        return Results.NotFound();

    return Results.Ok();
});

app.MapPost("/{listId:guid}/{itemId:guid}/delete", async (TodoListCommands commandHandler, Guid listId, Guid itemId) =>
{
    var result = await commandHandler.DeleteItem(listId, itemId);
    if (result == TodoListCommandResult.ListNotFound || result == TodoListCommandResult.ItemNotFound)
        return Results.NotFound();

    return Results.Ok();
});

app.MapGet("/{listId:guid}", async (ITodoListQuery queryHandler, Guid listId, int skip = 0, int take = 50) =>
{
    var result = await queryHandler.RetrieveListItems(listId, skip, take);
    if (result == null)
        return Results.NotFound();

    return Results.Ok(result);
});

app.MapGet("/{listId:guid}/{itemId:guid}", async (ITodoListQuery queryHandler, Guid listId, Guid itemId) =>
{
    var result = await queryHandler.RetrieveListItem(listId, itemId);
    if (result == null)
        return Results.NotFound();

    return Results.Ok(result);
});

app.Run();

record UpdateListItemDto(string title, string body);

