namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseSearchOut, Failure<int>>> SearchAsync(
        DataverseSearchIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        return cancellationToken.IsCancellationRequested ? 
            ValueTask.FromCanceled<Result<DataverseSearchOut, Failure<int>>>(cancellationToken) : 
            InnerSearchAsync(input, cancellationToken);
    }
    
    private async ValueTask<Result<DataverseSearchOut, Failure<int>>> InnerSearchAsync(
        DataverseSearchIn input, CancellationToken cancellationToken = default)
    {
        var httpClient = await DataverseHttpHelper
            .CreateHttpClientAsync(messageHandler, clientConfiguration, apiType: ApiTypeSearch, apiSearchType: ApiSearchType)
            .ConfigureAwait(false);

        var requestMessage = new HttpRequestMessage()
        { 
            Method = HttpMethod.Post,
            Content = DataverseHttpHelper.BuildRequestJsonBody(input.MapDataverseSearchIn()) 
        };
        var response = await httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<DataverseSearchJsonOut>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(DataverseHttpHelper.MapDataverseSearchJsonOut);
    }
}
