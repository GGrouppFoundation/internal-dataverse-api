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

    public static DataverseApiClient Create(HttpMessageHandler messageHandler, IFunc<DataverseApiClientConfiguration> configurationProvider)
        =>
        new (
            messageHandler ?? throw new ArgumentNullException(nameof(messageHandler)),
            configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider)));

    private readonly HttpMessageHandler messageHandler;

    private readonly IFunc<DataverseApiClientConfiguration> configurationProvider;

    private DataverseApiClient(HttpMessageHandler messageHandler, IFunc<DataverseApiClientConfiguration> configurationProvider)
    {
        this.messageHandler = messageHandler;
        this.configurationProvider = configurationProvider;
    }
}