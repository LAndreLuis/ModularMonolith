using System.Reflection;
using BuildingBlocks.Shared.Command;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Command
{
    public static class CommandRegistrationExtensions
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (type, interfaceType) => new { type, interfaceType })
            .Where(x => x.interfaceType.IsGenericType &&
                       x.interfaceType.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

            foreach (var handler in handlerTypes)
            {
                services.AddScoped(handler.interfaceType, handler.type);
            }

            return services;
        }

        public static IServiceCollection AddCommandDispatcher(this IServiceCollection services)
        {
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            return services;
        }
    }
}
