using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Shared.Command;

public interface ICommandDispatcher
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct = default);
}


public sealed class CommandDispatcher : ICommandDispatcher
{
    private delegate object HandlerInvoker(
        IServiceProvider provider,
        object command,
        CancellationToken ct);

    private static readonly ConcurrentDictionary<
        (Type CommandType, Type ResultType),
        Lazy<HandlerInvoker>> Invokers = new();

    private readonly IServiceProvider _serviceProvider;

    private static readonly MethodInfo GetRequiredServiceMethod =
        typeof(ServiceProviderServiceExtensions).GetMethod(
            nameof(ServiceProviderServiceExtensions.GetRequiredService),
            new[] { typeof(IServiceProvider), typeof(Type) })!;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> SendAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var commandType = command.GetType();

        var key = (commandType, typeof(TResult));

        var invoker = Invokers.GetOrAdd(
            key,
            static k => new Lazy<HandlerInvoker>(
                () => BuildInvoker(k.CommandType, k.ResultType),
                LazyThreadSafetyMode.ExecutionAndPublication
            )
        ).Value;

        return (Task<TResult>)invoker(
            _serviceProvider,
            command,
            ct);
    }

    private static HandlerInvoker BuildInvoker(
        Type commandType,
        Type resultType)
    {
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(commandType, resultType);

        var providerParameter =
            Expression.Parameter(typeof(IServiceProvider), "provider");

        var commandParameter =
            Expression.Parameter(typeof(object), "command");

        var ctParameter =
            Expression.Parameter(typeof(CancellationToken), "ct");

        // Resolve handler pelo DI
        var resolveHandler = Expression.Call(
            GetRequiredServiceMethod,
            providerParameter,
            Expression.Constant(handlerType)
        );

        var handler = Expression.Convert(
            resolveHandler,
            handlerType
        );

        var method = handlerType.GetMethod("HandleAsync")!;

        // Chamada direta ao método, sem MethodInfo.Invoke
        var call = Expression.Call(
            handler,
            method,
            Expression.Convert(commandParameter, commandType),
            ctParameter
        );

        var body = Expression.Convert(call, typeof(object));

        return Expression.Lambda<HandlerInvoker>(
            body,
            providerParameter,
            commandParameter,
            ctParameter
        ).Compile();
    }
}