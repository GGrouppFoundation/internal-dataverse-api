using System;
using System.Net.Http;

namespace GGroupp.Infra;

internal sealed partial class DataverseApiClient : IDataverseApiClient
{
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