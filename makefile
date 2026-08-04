lint-api:
	dotnet format TodoListApp.slnx

build-api:
	dotnet build TodoListApp.slnx

test-api:
	dotnet test src/api/TodoListApp.Tests/TodoListApp.Tests.csproj

