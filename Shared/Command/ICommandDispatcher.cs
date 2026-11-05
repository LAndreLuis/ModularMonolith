using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Shared.Command;

public interface ICommandDispatcher
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct = default);
}

public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // 1. Tipo CONCRETO do command (CreateProductCommand, DeleteProductCommand, ...)
        var commandType = command.GetType();

        // 2. Tipo do handler: ICommandHandler<CreateProductCommand, Guid>
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(commandType, typeof(TResult));

        // 3. Resolve do DI
        var handler = _serviceProvider.GetRequiredService(handlerType);

        // 4. Chama HandleAsync via reflection
        var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync))!;
        return (Task<TResult>)method.Invoke(handler, new object[] { command, ct })!;
    }
}