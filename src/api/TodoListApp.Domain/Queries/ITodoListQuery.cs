namespace TodoListApp.Domain.Queries;

/// Read-model representing a single item in the TODO list.
/// ID: UUID of the item within the list.
/// Sequence: Display order for the item.
/// Body: The content of the item.
public record TodoListItem(Guid itemID, int sequence, string title, string body);

public record RetrieveListItemsResult(TodoListItem[] items, int totalAvailable);

public interface ITodoListQuery
{
    /// Retrieves the items belonging to a specified list.
    /// listID: The UUID of the list to search for.
    /// Returns: The items from the specified list, or null if the specified list is not found.
    Task<RetrieveListItemsResult?> RetrieveListItems(Guid listID, int skip, int take);

    /// Retrieves the individual list item specified.
    /// Returns: The specified item, or null if it is not found.
    Task<TodoListItem?> RetrieveListItem(Guid listID, Guid itemID);
}
