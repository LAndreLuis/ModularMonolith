using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Shared.Query;

public static class QueryRegistrationExtensions
{
    
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface)
                continue;

            foreach (var interfaceType in type.GetInterfaces())
            {
                if (!interfaceType.IsGenericType)
                    continue;

                if (interfaceType.GetGenericTypeDefinition()
                    != typeof(IQueryHandler<,>))
                    continue;

                services.AddScoped(interfaceType, type);
            }
        }

        return services;
    }

   
    public static IServiceCollection AddQueryDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        return services;
    }
}