using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Query;

namespace BuildingBlocks.Shared.Query;

public static class QueryRegistrationExtensions
{
    
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (type, interfaceType) => new { type, interfaceType })
            .Where(x => x.interfaceType.IsGenericType &&
                       x.interfaceType.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

        foreach (var handler in handlerTypes)
        {
            services.AddScoped(handler.interfaceType, handler.type);
        }

        return services;
    }

   
    public static IServiceCollection AddQueryDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        return services;
    }
}