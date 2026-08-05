using Microsoft.Extensions.DependencyInjection;
namespace TodoListApp.Domain;

public static class Startup
{
    public static void RegisterDomainServices(this IServiceCollection services)
    {
        services.AddScoped<TodoListApp.Domain.Commands.TodoListCommands>();
    }
}
