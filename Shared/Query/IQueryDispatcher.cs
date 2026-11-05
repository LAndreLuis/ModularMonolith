using BuildingBlocks.Shared.Query;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Shared.Query;

public interface IQueryDispatcher
{
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken ct = default);
}

public sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(queryType, typeof(TResult));

        var handler = _serviceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))!;
        return (Task<TResult>)method.Invoke(handler, new object[] { query, ct })!;
    }
}