using Microsoft.Extensions.DependencyInjection;
using TodoListApp.Domain.Contracts;
using TodoListApp.Domain.Queries;

namespace TodoListApp.Infrastructure.InMemoryDB;

public static class Startup
{
    public static void RegisterInMemoryDb(this IServiceCollection services)
    {
        services.AddSingleton<TodoListInMemoryDb>();
        services.AddSingleton<ITodoListRepository>(sp => sp.GetRequiredService<TodoListInMemoryDb>());
        services.AddSingleton<ITodoListQuery>(sp => sp.GetRequiredService<TodoListInMemoryDb>());
    }
}
