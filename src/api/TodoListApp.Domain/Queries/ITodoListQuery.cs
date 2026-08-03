namespace TodoListApp.Domain.Queries;

public record TodoListItem(Guid itemID, int sequence, string body);

public interface ITodoListQuery
{
  /// Retrieves the items belonging to a specified list.
  /// listID: The UUID of the list to search for.
  /// Returns: The items from the specified list, or null if the specified list is not found.
  Task<TodoListItem[]> RetrieveListItems(Guid listID);
}
