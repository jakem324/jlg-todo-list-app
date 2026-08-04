using TodoListApp.Domain.Contracts;

namespace TodoListApp.Domain.Commands;

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
}

