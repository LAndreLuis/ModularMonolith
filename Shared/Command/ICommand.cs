namespace BuildingBlocks.Shared.Command;


public readonly record struct Unit
{
    public static readonly Unit Value = new();
}


public interface ICommand<out TResult> { }


public interface ICommand : ICommand<Unit> { }


public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken ct = default);
}

public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>
{ }