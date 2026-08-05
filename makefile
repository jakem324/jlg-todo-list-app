lint-api:
	dotnet format TodoListApp.slnx

restore-api-packages:
	dotnet restore TodoListApp.slnx

build-api:
	dotnet build TodoListApp.slnx

test-api:
	dotnet test src/api/TodoListApp.Tests/TodoListApp.Tests.csproj

#https://stackoverflow.com/questions/31024268/starting-and-closing-applications-in-makefile
start-api:
	@echo "Starting API in background..."
	@if [ -f api.pid ]; then \
		echo "API is already running."; \
	fi; \
	if [ ! -f api.pid ]; then \
		nohup dotnet run --project src/api/TodoListApp.Api/TodoListApp.Api.csproj > api.log 2>&1 & echo $$! > api.pid \
		echo "API process detached."; \
	fi;

stop-api:
	@echo "Stopping API..."
	@if [ -f api.pid ]; then \
    kill -TERM $$(cat api.pid) || true; \
	fi; \
	if [ ! -f api.pid ]; then \
		echo "API is not running."; \
	fi;
	@rm -f api.pid;
