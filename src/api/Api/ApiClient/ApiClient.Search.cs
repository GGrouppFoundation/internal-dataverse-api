using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseSearchOut, Failure<DataverseFailureCode>>> SearchAsync(
        DataverseSearchIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseSearchOut>(cancellationToken);
        }

        return InnerSearchAsync(input, cancellationToken);
    }
    
    private async ValueTask<Result<DataverseSearchOut, Failure<DataverseFailureCode>>> InnerSearchAsync(
        DataverseSearchIn input, CancellationToken cancellationToken)
    {
        using var httpClient = CreateSearchHttpClient();

        var searchIn = input.MapDataverseSearchIn();
        var requestMessage = new HttpRequestMessage()
        { 
            Method = HttpMethod.Post,
            Content = DataverseHttpHelper.BuildRequestJsonBody(searchIn) 
        };

        var response = await httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<DataverseSearchJsonOut>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(DataverseHttpHelper.MapDataverseSearchJsonOut);
    }
}
