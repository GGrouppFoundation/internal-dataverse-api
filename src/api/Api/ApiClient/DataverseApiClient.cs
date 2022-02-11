using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal sealed partial class DataverseApiClient : IDataverseApiClient
{
    private const string ApiVersionData = "9.1";

    private const string ApiTypeData = "data";

    private const string ApiVersionSearch = "1.0";

    private const string ApiTypeSearch = "search";

    private const string ApiSearchType = "query";

    private readonly HttpMessageHandler messageHandler;

    private readonly Uri dataverseBaseUri;

    internal DataverseApiClient(HttpMessageHandler messageHandler, Uri dataverseBaseUri)
    {
        this.messageHandler = messageHandler;
        this.dataverseBaseUri = dataverseBaseUri;
    }

    private HttpClient CreateDataHttpClient()
        =>
        new(messageHandler, disposeHandler: false)
        {
            BaseAddress = new(dataverseBaseUri, $"/api/{ApiTypeData}/v{ApiVersionData}/")
        };

    private HttpClient CreateSearchHttpClient()
        =>
        new(messageHandler, disposeHandler: false)
        {
            BaseAddress = new(dataverseBaseUri, $"/api/{ApiTypeSearch}/v{ApiVersionSearch}/{ApiSearchType}")
        };

    private static ValueTask<Result<T, Failure<DataverseFailureCode>>> GetCanceledAsync<T>(CancellationToken cancellationToken)
        =>
        ValueTask.FromCanceled<Result<T, Failure<DataverseFailureCode>>>(cancellationToken);
}