using System.Collections.Concurrent;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Shared.Query;

public interface IQueryDispatcher
{
    Task<TResult> SendAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken ct = default);
}

public sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly ConcurrentDictionary<
        (Type QueryType, Type ResultType),
        Lazy<object>> Cache = new();

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> SendAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var queryType = query.GetType();

        var key = (queryType, typeof(TResult));

        var lazyExecutor = Cache.GetOrAdd(
            key,
            _ => new Lazy<object>(
                () => CreateExecutor<TResult>(queryType),
                LazyThreadSafetyMode.ExecutionAndPublication));

        var executor = (Func<
            IServiceProvider,
            IQuery<TResult>,
            CancellationToken,
            Task<TResult>>)lazyExecutor.Value;

        return executor(_serviceProvider, query, ct);
    }

    private static Func<
        IServiceProvider,
        IQuery<TResult>,
        CancellationToken,
        Task<TResult>>
        CreateExecutor<TResult>(Type queryType)
    {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(queryType, typeof(TResult));

        var provider = Expression.Parameter(
            typeof(IServiceProvider), "provider");

        var query = Expression.Parameter(
            typeof(IQuery<TResult>), "query");

        var ct = Expression.Parameter(
            typeof(CancellationToken), "ct");

        var getHandlerMethod = typeof(ServiceProviderServiceExtensions)
            .GetMethod(
                nameof(ServiceProviderServiceExtensions.GetRequiredService),
                new[] { typeof(IServiceProvider), typeof(Type) })!;

        var getHandler = Expression.Call(
            getHandlerMethod,
            provider,
            Expression.Constant(handlerType));

        var typedHandler = Expression.Convert(
            getHandler, handlerType);

        var typedQuery = Expression.Convert(
            query, queryType);

        var handleMethod = handlerType.GetMethod("HandleAsync")!;

        var call = Expression.Call(
            typedHandler,
            handleMethod,
            typedQuery,
            ct);

        return Expression.Lambda<Func<
            IServiceProvider,
            IQuery<TResult>,
            CancellationToken,
            Task<TResult>>>(
                call,
                provider,
                query,
                ct)
            .Compile();
    }
}
