using BuildingBlocks.Shared.Command;

namespace Shared.Command
{
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
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType);

            if (handler is null)
            {
                throw new InvalidOperationException($"Nenhum handler foi registado para {command.GetType().Name}");
            }

            
            return ((dynamic)handler).HandleAsync((dynamic)command, ct);
        }
    }
    }
