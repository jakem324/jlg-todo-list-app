using Microsoft.Extensions.DependencyInjection;
using TodoListApp.Domain.Contracts;
using TodoListApp.Domain.Queries;

namespace TodoListApp.Infrastructure.InMemoryDB;

public static class Startup
{
    public static void RegisterInMemoryDb(this IServiceCollection services)
    {
        services.AddTransient<ITodoListRepository, TodoListInMemoryDb>();
        services.AddTransient<ITodoListQuery, TodoListInMemoryDb>();
    }
}
