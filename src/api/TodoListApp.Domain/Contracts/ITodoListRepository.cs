namespace TodoListApp.Domain.Contracts;

/// DTO model for creating or updating an item from the TODO list.
public record TodoListItem(string title, string body);

/// DTO model for persisting changes to an existing TODO list.
/// itemsToCreate: New items to add to the TODO list.
/// itemsToUpdate: Changes to apply to existing items within the TODO list.
/// itemsToDelete: Existing items within the TODO list to be removed.
public record TodoListChanges(
  IEnumerable<TodoListItem>? itemsToCreate = null,
  Dictionary<Guid, TodoListItem>? itemsToUpdate = null,
  IEnumerable<Guid>? itemsToDelete = null);

/// Result object representing the changes applied. Indicates the success of each
/// requested creation, update, and deletion, indicating "not found" errors for any
/// where applicable.
/// listUuidValid: indicates that the specified TODO list was found.
/// created: UUIDs of the newly-created item(s).
/// updated: UUIDs of existing items found and updated. Exclusion of a requested
/// item from this list indicates that the given item UUID was not found.
/// deleted: UUIDs of existing items found and deleted. Exclusion of a requested
/// item from this list indicates that the given item UUID was not found.
public record TodoListChangesResult(
  bool listUuidValid,
  IEnumerable<Guid>? created = null,
  IEnumerable<Guid>? updated = null,
  IEnumerable<Guid>? deleted = null);

public interface ITodoListRepository
{
    /// Initialises a new list.
    /// Returns: Auto-generated UUID to be used for item commitment and retrieval.
    Task<Guid> InitializeNewList();
    /// Writes to the specified list, overwriting the existing list items.
    /// listID: The UUID of the list to write over.
    /// changes: The changes to apply.
    Task<TodoListChangesResult> CommitListChanges(Guid listID, TodoListChanges changes);
}
