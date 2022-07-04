using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal sealed partial class DataverseApiClient : IDataverseApiClient
{
    private const string ApiVersionData = "9.2";

    private const string ApiTypeData = "data";

    private const string ApiVersionSearch = "1.0";

    private const string ApiTypeSearch = "search";

    private const string ApiSearchType = "query";

    private const string CallerIdHeaderName = "MSCRMCallerID";

    private readonly HttpMessageHandler messageHandler;

    private readonly Uri dataverseBaseUri;

    private readonly Guid? callerId;

    internal DataverseApiClient(HttpMessageHandler messageHandler, Uri dataverseBaseUri)
    {
        this.messageHandler = messageHandler;
        this.dataverseBaseUri = dataverseBaseUri;
    }

    private DataverseApiClient(HttpMessageHandler messageHandler, Uri dataverseBaseUri, Guid callerId)
    {
        this.messageHandler = messageHandler;
        this.dataverseBaseUri = dataverseBaseUri;
        this.callerId = callerId;
    }

    private HttpClient CreateDataHttpClient()
        =>
        CreateHttpClient($"/api/{ApiTypeData}/v{ApiVersionData}/");

    private HttpClient CreateSearchHttpClient()
        =>
        CreateHttpClient($"/api/{ApiTypeSearch}/v{ApiVersionSearch}/{ApiSearchType}");

    private HttpClient CreateHttpClient(string relativeUrl)
    {
        var httpClient = new HttpClient(messageHandler, disposeHandler: false)
        {
            BaseAddress = new(dataverseBaseUri, relativeUrl)
        };

        if (callerId is not null)
        {
            httpClient.DefaultRequestHeaders.Add(CallerIdHeaderName, callerId.Value.ToString("D", CultureInfo.InvariantCulture));
        }

        return httpClient;
    }

    private static ValueTask<Result<T, Failure<DataverseFailureCode>>> GetCanceledAsync<T>(CancellationToken cancellationToken)
        =>
        ValueTask.FromCanceled<Result<T, Failure<DataverseFailureCode>>>(cancellationToken);
}