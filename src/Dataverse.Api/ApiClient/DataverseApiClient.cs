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

    public static DataverseApiClient Create(HttpMessageHandler messageHandler, IDataverseApiClientConfiguration clientConfiguration)
        =>
        new(
            messageHandler ?? throw new ArgumentNullException(nameof(messageHandler)),
            clientConfiguration ?? throw new ArgumentNullException(nameof(clientConfiguration)));

    private readonly HttpMessageHandler messageHandler;

    private readonly IDataverseApiClientConfiguration clientConfiguration;

    private DataverseApiClient(HttpMessageHandler messageHandler, IDataverseApiClientConfiguration clientConfiguration)
    {
        this.messageHandler = messageHandler;
        this.clientConfiguration = clientConfiguration;
    }
}