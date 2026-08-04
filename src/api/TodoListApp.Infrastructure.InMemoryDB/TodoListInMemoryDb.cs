using Command = TodoListApp.Domain.Contracts;
using Query = TodoListApp.Domain.Queries;

namespace TodoListApp.Infrastructure.InMemoryDB;

public class TodoListInMemoryDb : Command.ITodoListRepository, Query.ITodoListQuery
{
    public Task<Guid> InitializeNewList()
    {
        throw new NotImplementedException();
    }

    public Task<Command.TodoListChangesResult> CommitListChanges(Guid listID, Command.TodoListChanges changes)
    {
        throw new NotImplementedException();
    }

    public Task<Query.RetrieveListItemsResult> RetrieveListItems(Guid listID, int skip, int take)
    {
        throw new NotImplementedException();
    }
}
