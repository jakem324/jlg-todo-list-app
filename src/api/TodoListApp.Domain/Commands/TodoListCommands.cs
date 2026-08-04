using TodoListApp.Domain.Contracts;

namespace TodoListApp.Domain.Commands;

public enum TodoListCommandResult
{
    Ok,
    ListNotFound,
    ItemNotFound
}

public class TodoListCommands
{
    private readonly ITodoListRepository _todoListRepository;

    public TodoListCommands(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Guid> InitializeNewTodoList()
    {
        var uuid = await _todoListRepository.InitializeNewList();
        return uuid;
    }

    public async Task<(TodoListCommandResult, Guid)> InitializeNewItem(Guid listUuid)
    {
        var result = await _todoListRepository.CommitListChanges(
          listUuid,
          new TodoListChanges(
            itemsToCreate: new[] {
              new TodoListItem(
                title: string.Empty,
                body: string.Empty)
            }));
        if (!result.listUuidValid)
        {
            return (TodoListCommandResult.ListNotFound, Guid.Empty);
        }

        var createdItemId = result.created.First();
        return (TodoListCommandResult.Ok, createdItemId);
    }

    public async Task<TodoListCommandResult> UpdateItem(Guid listUuid, Guid itemId, string title, string body)
    {
        var result = await _todoListRepository.CommitListChanges(
          listUuid,
          new TodoListChanges(
            itemsToUpdate: new Dictionary<Guid, TodoListItem>
            {
                [itemId] = new TodoListItem(
                  title: title,
                  body: body)
            }));
        if (!result.listUuidValid)
        {
            return TodoListCommandResult.ListNotFound;
        }

        if (!result.updated.Contains(itemId))
        {
            return TodoListCommandResult.ItemNotFound;
        }

        return TodoListCommandResult.Ok;
    }

    public async Task<TodoListCommandResult> DeleteItem(Guid listUuid, Guid itemId)
    {
        var result = await _todoListRepository.CommitListChanges(
          listUuid,
          new TodoListChanges(
            itemsToDelete: new[] { itemId }));
        if (!result.listUuidValid)
        {
            return TodoListCommandResult.ListNotFound;
        }

        if (!result.deleted.Contains(itemId))
        {
            return TodoListCommandResult.ItemNotFound;
        }

        return TodoListCommandResult.Ok;
    }
}

