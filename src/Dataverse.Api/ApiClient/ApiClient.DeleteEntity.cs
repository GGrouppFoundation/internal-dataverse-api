#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra
{
    partial class DataverseApiClient
    {
        public ValueTask<Result<Unit, Failure<int>>> DeleteEntityAsync(
            DataverseEntityDeleteIn input, CancellationToken cancellationToken = default)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<Result<Unit, Failure<int>>>(cancellationToken);
            }

            return InternalDeleteEntityAsync(input, cancellationToken);
        }

        private async ValueTask<Result<Unit, Failure<int>>> InternalDeleteEntityAsync(
            DataverseEntityDeleteIn input, CancellationToken cancellationToken = default)
        {
            var httpClient = await DataverseHttpHelper.CreateHttpClientAsync(messageHandler, clientConfiguration);

            var entitiyDeleteUrl = $"{input.EntityPluralName}({input.EntityKey.Value})";

            var response = await httpClient.DeleteAsync(entitiyDeleteUrl, cancellationToken).ConfigureAwait(false);
            return await response.ReadDataverseResultAsync<Unit>(cancellationToken).ConfigureAwait(false);
        }  
    }
}