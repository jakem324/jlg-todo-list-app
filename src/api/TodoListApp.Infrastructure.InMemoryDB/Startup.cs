using Microsoft.Extensions.DependencyInjection;
using TodoListApp.Domain.Contracts;

namespace TodoListApp.Infrastructure.InMemoryDB;

public static class Startup
{
    public static void RegisterInMemoryDb(this IServiceCollection services)
    {
        services.AddTransient<ITodoListRepository, TodoListInMemoryDb>();
    }
}
