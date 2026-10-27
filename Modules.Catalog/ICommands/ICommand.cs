namespace Modules.Catalog.ICommands
{
    public class ICommands
    {
        public interface ICommand<out TResult> { }

        // Representa o executor da lógica de negócio.
        public interface ICommandHandler<in TCommand, TResult>
            where TCommand : ICommand<TResult>
        {
            Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
        }
    }
}
