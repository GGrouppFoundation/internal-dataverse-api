using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseSearchOut, Failure<int>>> SearchAsync(
        DataverseSearchIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        return cancellationToken.IsCancellationRequested
            ? ValueTask.FromCanceled<Result<DataverseSearchOut, Failure<int>>>(cancellationToken)
            : InnerSearchAsync(input, cancellationToken);
    }
    
    private async ValueTask<Result<DataverseSearchOut, Failure<int>>> InnerSearchAsync(
        DataverseSearchIn input, CancellationToken cancellationToken)
    {
        using var httpClient = await DataverseHttpHelper.InternalCreateHttpClientAsync(
                messageHandler,
                configurationProvider.Invoke(),
                apiVersion: ApiVersionSearch,
                apiType: ApiTypeSearch,
                apiSearchType: ApiSearchType)
            .ConfigureAwait(false);

        var searchIn = input.InternalMapDataverseSearchIn();
        var requestMessage = new HttpRequestMessage()
        { 
            Method = HttpMethod.Post,
            Content = DataverseHttpHelper.InternalBuildRequestJsonBody(searchIn) 
        };

        var response = await httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        var result = await response.InternalReadDataverseResultAsync<DataverseSearchJsonOut>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(DataverseHttpHelper.InternalMapDataverseSearchJsonOut);
    }
}
