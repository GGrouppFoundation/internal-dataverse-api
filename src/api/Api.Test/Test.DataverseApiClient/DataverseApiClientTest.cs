using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;

namespace GGroupp.Infra.Dataverse.Api.Test;

public sealed partial class DataverseApiClientTest
{
    private static readonly Uri SomeDataverseBaseUri
        =
        new("https://some.crm4.dynamics.com/", UriKind.Absolute);

    private static readonly DataverseEntityGetIn SomeDataverseEntityGetInput
        =
        new(
            entityPluralName: "SomeEntities",
            entityKey: new StubEntityKey("Some key"),
            selectFields: new[] { "Some field name" });

    private static IDataverseApiClient CreateDataverseApiClient(HttpMessageHandler messageHandler, Uri dataverseBaseUri)
        =>
        new DataverseApiClient(messageHandler, dataverseBaseUri);

    private static Mock<IAsyncFunc<HttpRequestMessage, HttpResponseMessage>> CreateMockProxyHandler(
        HttpResponseMessage responseMessage, Action<HttpRequestMessage>? callback = default)
    {
        var mock = new Mock<IAsyncFunc<HttpRequestMessage, HttpResponseMessage>>();

        var m = mock
            .Setup(p => p.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(responseMessage));

        if (callback is not null)
        {
            _ = m.Callback<HttpRequestMessage, CancellationToken>(
                (r, _) => callback.Invoke(r));
        }

        return mock;
    }
}