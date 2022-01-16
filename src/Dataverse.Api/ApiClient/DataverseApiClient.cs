using System;
using System.Net.Http;

namespace GGroupp.Infra;

internal sealed partial class DataverseApiClient : IDataverseApiClient
{
    private const string ApiVersionData = "9.0";

    private const string ApiTypeData = "data";

    private const string ApiVersionSearch = "1.0";

    private const string ApiTypeSearch = "search";

    private const string ApiSearchType = "query";

    public static DataverseApiClient Create(HttpMessageHandler messageHandler, DataverseApiClientConfiguration configuration)
        =>
        new (
            messageHandler ?? throw new ArgumentNullException(nameof(messageHandler)),
            configuration ?? throw new ArgumentNullException(nameof(configuration)));

    private readonly HttpMessageHandler messageHandler;

    private readonly DataverseApiClientConfiguration configuration;

    private DataverseApiClient(HttpMessageHandler messageHandler, DataverseApiClientConfiguration configuration)
    {
        this.messageHandler = messageHandler;
        this.configuration = configuration;
    }
}