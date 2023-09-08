using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal static class ResultExtensions
{
    internal static ValueTask<Result<TNextSuccess, TFailure>> ForwardValueAsync<TSuccess, TNextSuccess, TFailure>(
        this Result<TSuccess, TFailure> result,
        Func<TSuccess, CancellationToken, ValueTask<Result<TNextSuccess, TFailure>>> nextAsync,
        CancellationToken cancellationToken)
        where TFailure : struct
    {
        return result.ForwardValueAsync(InnerNextAsync);

        ValueTask<Result<TNextSuccess, TFailure>> InnerNextAsync(TSuccess success)
            =>
            nextAsync.Invoke(success, cancellationToken);
    }
}