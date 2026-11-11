using System.Reflection;
using BuildingBlocks.Shared.Command;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Command;

public static class CommandRegistrationExtensions
{
    public static IServiceCollection AddCommandHandlers(
        this IServiceCollection services,
        Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        foreach (var type in assembly.GetTypes())
        {
            if (!type.IsClass || type.IsAbstract)
                continue;

            foreach (var interfaceType in type.GetInterfaces())
            {
                if (!interfaceType.IsGenericType)
                    continue;

                if (interfaceType.GetGenericTypeDefinition() !=
                    typeof(ICommandHandler<,>))
                    continue;

                services.AddScoped(interfaceType, type);
            }
        }

        return services;
    }

    public static IServiceCollection AddCommandDispatcher(
        this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();

        return services;
    }
}