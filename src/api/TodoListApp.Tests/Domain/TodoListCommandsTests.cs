using TestStack.BDDfy;
using Moq;
using FluentAssertions;

using TodoListApp.Domain.Commands;
using TodoListApp.Domain.Contracts;

namespace TodoListApp.Tests.Domain;

public class TodoListCommandsTests
{
    [Fact]
    public void InitializeTodoList()
    {
        var newTodoListUuid = Guid.NewGuid();
        this.Given(_ => _.ARepositoryMock())
          .And(_ => _.TheRepositoryMockInitializeNewListReturns(newTodoListUuid))
          .When(_ => _.TheInitializeTodoListCommandIsCalled())
          .Then(_ => _.TheRepositoryMockInitializeNewListIsCalled())
          .And(_ => _.TheMockUuidIsReturned(newTodoListUuid))
          .BDDfy();
    }

    [Fact]
    public void AddItem_ValidListId()
    {
        var listId = Guid.NewGuid();
        var newItemId = Guid.NewGuid();
        var expectedChanges = new TodoListChanges(
            itemsToCreate: new[] {
              new TodoListItem(
                title: string.Empty,
                body: string.Empty)
            });

        this.Given(_ => _.ARepositoryMock())
          .And(_ => _.TheRepositoryMockIndicatesValidListUuid())
          .And(_ => _.TheRepositoryMockIndicatesItemCreated(newItemId))
          .When(_ => _.TheInitializeNewItemCommandIsCalled(listId))
          .Then(_ => _.TheRepositoryMockCommitListChangesIsCalled(listId, expectedChanges))
          .And(_ => _.TheReturnedCommandResultIs(TodoListCommandResult.Ok))
          .BDDfy();
    }

    [Fact]
    public void AddItem_InvalidListId()
    {
        var listId = Guid.NewGuid();
        var newItemId = Guid.NewGuid();
        var expectedChanges = new TodoListChanges(
            itemsToCreate: new[] {
              new TodoListItem(
                title: string.Empty,
                body: string.Empty)
            });

        this.Given(_ => _.ARepositoryMock())
          .And(_ => _.TheRepositoryMockIndicatesInvalidListUuid())
          .When(_ => _.TheInitializeNewItemCommandIsCalled(listId))
          .Then(_ => _.TheRepositoryMockCommitListChangesIsCalled(listId, expectedChanges))
          .And(_ => _.TheReturnedCommandResultIs(TodoListCommandResult.ListNotFound))
          .BDDfy();
    }

    [Fact]
    public void UpdateItem_ValidListId_ValidItemId()
    {
        var listId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var newTitle = "New title";
        var newBody = "New body content";

        var expectedChanges = new TodoListChanges(
            itemsToUpdate: new Dictionary<Guid, TodoListItem>
            {
                [itemId] = new TodoListItem(
                  title: newTitle,
                  body: newBody)
            });

        this.Given(_ => _.ARepositoryMock())
          .And(_ => _.TheRepositoryMockIndicatesValidListUuid())
          .And(_ => _.TheRepositoryMockIndicatesItemUpdated(itemId))
          .When(_ => _.TheUpdateItemCommandIsCalled(listId, itemId, newTitle, newBody))
          .Then(_ => _.TheRepositoryMockCommitListChangesIsCalled(listId, expectedChanges))
          .And(_ => _.TheReturnedCommandResultIs(TodoListCommandResult.Ok))
          .BDDfy();
    }

    [Fact]
    public void UpdateItem_ValidListId_InvalidItemId()
    {
        var listId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var newTitle = "New title";
        var newBody = "New body content";

        var expectedChanges = new TodoListChanges(
            itemsToUpdate: new Dictionary<Guid, TodoListItem>
            {
                [itemId] = new TodoListItem(
                  title: newTitle,
                  body: newBody)
            });

        this.Given(_ => _.ARepositoryMock())
          .And(_ => _.TheRepositoryMockIndicatesValidListUuid())
          // Item ID not listed as a successful update
          .When(_ => _.TheUpdateItemCommandIsCalled(listId, itemId, newTitle, newBody))
          .Then(_ => _.TheRepositoryMockCommitListChangesIsCalled(listId, expectedChanges))
          .And(_ => _.TheReturnedCommandResultIs(TodoListCommandResult.ItemNotFound))
          .BDDfy();
    }

    [Fact]
    public void UpdateItem_InvalidListId()
    {
        var listId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var newTitle = "New title";
        var newBody = "New body content";

        var expectedChanges = new TodoListChanges(
            itemsToUpdate: new Dictionary<Guid, TodoListItem>
            {
                [itemId] = new TodoListItem(
                  title: newTitle,
                  body: newBody)
            });

        this.Given(_ => _.ARepositoryMock())
          .And(_ => _.TheRepositoryMockIndicatesInvalidListUuid())
          .When(_ => _.TheUpdateItemCommandIsCalled(listId, itemId, newTitle, newBody))
          .Then(_ => _.TheRepositoryMockCommitListChangesIsCalled(listId, expectedChanges))
          .And(_ => _.TheReturnedCommandResultIs(TodoListCommandResult.ListNotFound))
          .BDDfy();
    }

    [Fact]
    public void DeleteItem_ValidListId_ValidItemId()
    {
        var listId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        var expectedChanges = new TodoListChanges(
            itemsToDelete: new[] { itemId });

        this.Given(_ => _.ARepositoryMock())
          .And(_ => _.TheRepositoryMockIndicatesValidListUuid())
          .And(_ => _.TheRepositoryMockIndicatesItemDeleted(itemId))
          .When(_ => _.TheDeleteItemCommandIsCalled(listId, itemId))
          .Then(_ => _.TheRepositoryMockCommitListChangesIsCalled(listId, expectedChanges))
          .And(_ => _.TheReturnedCommandResultIs(TodoListCommandResult.Ok))
          .BDDfy();
    }

    [Fact]
    public void DeleteItem_ValidListId_InvalidItemId()
    {
        var listId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        var expectedChanges = new TodoListChanges(
            itemsToDelete: new[] { itemId });

        this.Given(_ => _.ARepositoryMock())
          .And(_ => _.TheRepositoryMockIndicatesValidListUuid())
          // Item ID not listed as a successful deletion
          .When(_ => _.TheDeleteItemCommandIsCalled(listId, itemId))
          .Then(_ => _.TheRepositoryMockCommitListChangesIsCalled(listId, expectedChanges))
          .And(_ => _.TheReturnedCommandResultIs(TodoListCommandResult.ItemNotFound))
          .BDDfy();
    }

    [Fact]
    public void DeleteItem_InvalidListId()
    {
        var listId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        var expectedChanges = new TodoListChanges(
            itemsToDelete: new[] { itemId });

        this.Given(_ => _.ARepositoryMock())
          .And(_ => _.TheRepositoryMockIndicatesInvalidListUuid())
          .When(_ => _.TheDeleteItemCommandIsCalled(listId, itemId))
          .Then(_ => _.TheRepositoryMockCommitListChangesIsCalled(listId, expectedChanges))
          .And(_ => _.TheReturnedCommandResultIs(TodoListCommandResult.ListNotFound))
          .BDDfy();
    }


    #region BDD
    #region Data
    private TodoListCommands _sut;
    private Mock<ITodoListRepository> _todoListRepository;
    private TodoListChangesResult _todoListChangesResult;
    private TodoListChanges _changesInvoked;

    private Guid _initializeNewListResult;
    private TodoListCommandResult _commandResult;
    private Guid _commandResultGuid;
    #endregion
    #region Given
    private void ARepositoryMock()
    {
        _todoListRepository = new Mock<ITodoListRepository>();
        _todoListChangesResult = new TodoListChangesResult(
            listUuidValid: true,
            created: Array.Empty<Guid>(),
            updated: Array.Empty<Guid>(),
            deleted: Array.Empty<Guid>()
        );
    }

    private void TheRepositoryMockInitializeNewListReturns(Guid generatedUuid)
    {
        _todoListRepository.Setup(_ => _.InitializeNewList()).ReturnsAsync(generatedUuid);
    }

    private void TheRepositoryMockIndicatesValidListUuid()
    {
        _todoListChangesResult = _todoListChangesResult with
        {
            listUuidValid = true
        };
    }

    private void TheRepositoryMockIndicatesInvalidListUuid()
    {
        _todoListChangesResult = _todoListChangesResult with
        {
            listUuidValid = false
        };
    }

    private void TheRepositoryMockIndicatesItemCreated(Guid newItemId)
    {
        _todoListChangesResult = _todoListChangesResult with
        {
            created = _todoListChangesResult.created!.Append(newItemId)
        };
    }

    private void TheRepositoryMockIndicatesItemUpdated(Guid itemId)
    {
        _todoListChangesResult = _todoListChangesResult with
        {
            updated = _todoListChangesResult.updated!.Append(itemId)
        };
    }

    private void TheRepositoryMockIndicatesItemDeleted(Guid itemId)
    {
        _todoListChangesResult = _todoListChangesResult with
        {
            deleted = _todoListChangesResult.deleted!.Append(itemId)
        };
    }
    #endregion
    #region When
    private void InitializeSut()
    {
        _todoListRepository
          .Setup(_ => _.CommitListChanges(It.IsAny<Guid>(), It.IsAny<TodoListChanges>()))
          .Callback<Guid, TodoListChanges>((_, changes) => { _changesInvoked = changes; })
          .ReturnsAsync(_todoListChangesResult);
        _sut = new TodoListCommands(_todoListRepository.Object);
    }

    private async Task TheInitializeTodoListCommandIsCalled()
    {
        InitializeSut();
        _initializeNewListResult = await _sut.InitializeNewTodoList();
    }

    private async Task TheInitializeNewItemCommandIsCalled(Guid listId)
    {
        InitializeSut();
        (_commandResult, _commandResultGuid) = await _sut.InitializeNewItem(listId);
    }

    private async Task TheUpdateItemCommandIsCalled(Guid listId, Guid itemId, string title, string body)
    {
        InitializeSut();
        _commandResult = await _sut.UpdateItem(listId, itemId, title, body);
    }

    private async Task TheDeleteItemCommandIsCalled(Guid listId, Guid itemId)
    {
        InitializeSut();
        _commandResult = await _sut.DeleteItem(listId, itemId);
    }

    #endregion
    #region Then
    private void TheRepositoryMockInitializeNewListIsCalled()
    {
        _todoListRepository.Verify(_ => _.InitializeNewList());
    }

    private void TheRepositoryMockCommitListChangesIsCalled(Guid listId, TodoListChanges expectedChanges)
    {
        _changesInvoked.Should().BeEquivalentTo(expectedChanges);
    }

    private void TheMockUuidIsReturned(Guid uuid)
    {
        Assert.Equal(uuid, _initializeNewListResult);
    }

    private void TheReturnedCommandResultIs(TodoListCommandResult expected)
    {
        Assert.Equal(expected, _commandResult);
    }
    #endregion
    #endregion
}

