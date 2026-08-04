using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;
using Moq;

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

    #region BDD
    #region Data
    private TodoListCommands _sut;
    private Mock<ITodoListRepository> _todoListRepository;

    private Guid _initializeNewListResult;
    #endregion
    #region Given
    private void ARepositoryMock()
    {
        _todoListRepository = new Mock<ITodoListRepository>();
    }

    private void TheRepositoryMockInitializeNewListReturns(Guid generatedUuid)
    {
        _todoListRepository.Setup(_ => _.InitializeNewList()).ReturnsAsync(generatedUuid);
    }
    #endregion
    #region When
    private async Task TheInitializeTodoListCommandIsCalled()
    {
        _sut = new TodoListCommands(_todoListRepository.Object);
        _initializeNewListResult = await _sut.InitializeNewTodoList();
    }
    #endregion
    #region Then
    private void TheRepositoryMockInitializeNewListIsCalled()
    {
        _todoListRepository.Verify(_ => _.InitializeNewList());
    }

    private void TheMockUuidIsReturned(Guid uuid)
    {
        Assert.Equal(uuid, _initializeNewListResult);
    }
    #endregion
    #endregion
}

