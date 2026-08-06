# JLG Todo List App

## Dependencies
* .NET 10 SDK
* npm
* Angular CLI
* GNU `make` (optional)

## Quickstart

### If using `make`

1) Launch the API with `make start-api` *
2) Launch the UI with `make serve-ui`

(*) The .NET process will detach and run in the background. To stop it, run `make stop-api`. If running it via `make`, the API logs will be printed to `./api.log`.

### Without `make`

1) Launch the API with:
```
dotnet run --project src/api/TodoListApp.Api/TodoListApp.Api.csproj
```
2) Launch the UI with:
```
cd src/ui && npm run start
```

## Debugging
Visit the running UI at http://localhost:4200

The landing page will initialize an empty TODO list, which will be persisted to the backend and given an ID, ready for items to be added.

To view a TODO list filled with mock items, visit:

http://localhost:4200/3eb8ec4a-cd90-4923-94c6-8966e06f5e57

## Testing
To test the API, run `make test-api`. The UI may be tested using the standard Angular CLI commands, however, owing to time constraints, test coverage has not been prioritized on the UI.

## Linting
To run linting on the API, use `make lint-api`, or
```
dotnet format TodoListApp.slnx
```

To lint the UI, `make lint-ui`, or
```
cd src/ui && npx ng lint && npx prettier . --write
```

## A note on the architecture

This solution utilizes Domain-Driven Design with BDD-style tests on the backend. I realize that in many cases, both would be overkill for a simple CRUD app in production. For the purpose of this exercise, I have used this architecture solely to demonstrate my ability in architecting large systems, which will give us some additional talking points. I look forward to discussing and extending this work together in an interactive session.
