using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<Unit, Failure<int>>> DeleteEntityAsync(
        DataverseEntityDeleteIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        return cancellationToken.IsCancellationRequested
            ? ValueTask.FromCanceled<Result<Unit, Failure<int>>>(cancellationToken)
            : InnerDeleteEntityAsync(input, cancellationToken);
    }

    private async ValueTask<Result<Unit, Failure<int>>> InnerDeleteEntityAsync(
        DataverseEntityDeleteIn input, CancellationToken cancellationToken)
    {
        var httpClient = await DataverseHttpHelper
            .InternalCreateHttpClientAsync(
                messageHandler,
                configurationProvider.Invoke(),
                apiVersion: ApiVersionData,
                apiType: ApiTypeData)
            .ConfigureAwait(false);

        var entitiyDeleteUrl = $"{input.EntityPluralName}({input.EntityKey.Value})";

        var response = await httpClient.DeleteAsync(entitiyDeleteUrl, cancellationToken).ConfigureAwait(false);
        return await response.InternalReadDataverseResultAsync<Unit>(cancellationToken).ConfigureAwait(false);
    }  
}