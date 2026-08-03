namespace TodoListApp.Domain.Contracts;

/// DTO model for creating or updating an item from the TODO list.
public record TodoListItem(string body);

/// DTO model for persisting changes to an existing TODO list.
/// itemsToCreate: New items to add to the TODO list.
/// itemsToUpdate: Changes to apply to existing items within the TODO list.
/// itemsToDelete: Existing items within the TODO list to be removed.
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
