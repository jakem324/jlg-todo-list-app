namespace TodoListApp.Domain.Contracts;

public record TodoListItem(int sequence, string body);
public record TodoListChanges(
  IEnumerable<TodoListItem> itemsToCreate,
  Dictionary<Guid, TodoListItem> itemsToUpdate,
  IEnumerable<Guid> itemsToDelete);

public interface ITodoListRepository
{
  /// Initialises a new list.
  /// Returns: Auto-generated UUID to be used for item commitment and retrieval.
  Task<Guid> InitializeNewList();
  /// Writes to the specified list, overwriting the existing list items.
  /// listID: The UUID of the list to write over.
  /// items: The new set of items to write.
  /// Returns: true if the specified list UUID was found; false if not.
  Task<bool> CommitListChanges(Guid listID, IEnumerable<string> items);
}
