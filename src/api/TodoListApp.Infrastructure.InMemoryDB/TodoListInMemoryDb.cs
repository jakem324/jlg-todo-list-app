using Command = TodoListApp.Domain.Contracts;
using Query = TodoListApp.Domain.Queries;

namespace TodoListApp.Infrastructure.InMemoryDB;

/// A crude in-memory DB implementation. Not intended to scale.
public class TodoListInMemoryDb : Command.ITodoListRepository, Query.ITodoListQuery
{
    private Dictionary<Guid, Query.TodoListItem[]> _lists;
    public TodoListInMemoryDb()
    {
        _lists = new Dictionary<Guid, Query.TodoListItem[]>();
        SeedMockData();
    }

    public Task<Guid> InitializeNewList()
    {
        var uuid = Guid.NewGuid();
        _lists.Add(uuid, Array.Empty<Query.TodoListItem>());
        return Task.FromResult(uuid);
    }

    public Task<Command.TodoListChangesResult> CommitListChanges(Guid listID, Command.TodoListChanges changes)
    {
        if (!_lists.ContainsKey(listID))
        {
            return Task.FromResult(new Command.TodoListChangesResult(listUuidValid: false));
        }

        var seq = _lists[listID].Max(x => x.sequence);
        var created = new List<Guid>();
        var itemsToCreate = changes.itemsToCreate ?? Array.Empty<Command.TodoListItem>();
        foreach (var addition in itemsToCreate)
        {
            var uuid = Guid.NewGuid();
            var item = new Query.TodoListItem(itemID: uuid, sequence: seq + 1, title: addition.title, body: addition.body);
            _lists[listID] = _lists[listID].Append(item).ToArray();
            created.Add(uuid);
            seq++;
        }

        var updated = new List<Guid>();
        var itemsToUpdate = changes.itemsToUpdate ?? new Dictionary<Guid, Command.TodoListItem>();
        foreach (var update in itemsToUpdate)
        {
            var itemsInList = _lists[listID];
            var relevantItem = itemsInList.Where(x => x.itemID == update.Key).FirstOrDefault();
            if (relevantItem == null)
                continue;
            _lists[listID] = itemsInList
              .Select(existingItem => existingItem.itemID == update.Key
                ? existingItem with { title = update.Value.title, body = update.Value.body }
                : existingItem)
              .ToArray();
            updated.Add(update.Key);
        }

        var deleted = new List<Guid>();
        var itemsToDelete = changes.itemsToDelete ?? Array.Empty<Guid>();
        foreach (var deletion in itemsToDelete)
        {
            var itemsInList = _lists[listID];
            var relevantItem = itemsInList.Where(x => x.itemID == deletion).FirstOrDefault();
            if (relevantItem == null)
                continue;
            _lists[listID] = itemsInList
              .Where(existingItem => existingItem.itemID != deletion)
              .ToArray();
            deleted.Add(deletion);
        }

        return Task.FromResult(new Command.TodoListChangesResult(
            listUuidValid: false,
            created: created,
            updated: updated,
            deleted: deleted));
    }

    public Task<Query.RetrieveListItemsResult?> RetrieveListItems(Guid listID, int skip, int take)
    {
        if (!_lists.ContainsKey(listID))
            return null!;
        return Task.FromResult(new Query.RetrieveListItemsResult(
              _lists[listID].Skip(skip).Take(take).ToArray(),
              _lists[listID].Length))!;
    }

    private void SeedMockData()
    {
        var faker = new Bogus.Faker();
        var items = Enumerable.Range(0, 200).Select(index => new Query.TodoListItem(
          itemID: Guid.NewGuid(),
          title: $"TODO: {faker.Hacker.Verb()} {faker.Hacker.Noun()}",
          body: $"Need to {faker.Hacker.Verb().ToLower()} the {faker.Hacker.Noun()} in order to {faker.Hacker.Verb().ToLower()}",
          sequence: index
        )).ToArray();

        _lists.Add(Guid.Parse("3eb8ec4a-cd90-4923-94c6-8966e06f5e57"), items);
    }
}
