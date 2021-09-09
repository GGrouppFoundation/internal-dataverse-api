using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<Unit, Failure<int>>> DeleteEntityAsync(
        DataverseEntityDeleteIn input, CancellationToken cancellationToken = default)
        =>
        (input, cancellationToken.IsCancellationRequested) switch
        {
            (null, _) => throw new ArgumentNullException(nameof(input)),
            (_, true) => ValueTask.FromCanceled<Result<Unit, Failure<int>>>(cancellationToken),
            _ => InternalDeleteEntityAsync(input, cancellationToken)
        };

    private async ValueTask<Result<Unit, Failure<int>>> InternalDeleteEntityAsync(
        DataverseEntityDeleteIn input, CancellationToken cancellationToken = default)
    {
        var httpClient = await DataverseHttpHelper.CreateHttpClientAsync(messageHandler, clientConfiguration);

        var entitiyDeleteUrl = $"{input.EntityPluralName}({input.EntityKey.Value})";

        var response = await httpClient.DeleteAsync(entitiyDeleteUrl, cancellationToken).ConfigureAwait(false);
        return await response.ReadDataverseResultAsync<Unit>(cancellationToken).ConfigureAwait(false);
    }  
}