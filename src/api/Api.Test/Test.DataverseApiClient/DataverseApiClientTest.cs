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

    private static readonly DataverseEntitySetGetIn SomeDataverseEntitySetGetInput
        =
        new(
            entityPluralName: "SomeEntities",
            selectFields: new[] { "Some field name" },
            filter: "Some filter",
            orderBy: new DataverseOrderParameter[] { new("one", DataverseOrderDirection.Default) },
            top: 5);

    private static readonly DataverseEntityUpdateIn<StubRequestJson> SomeDataverseEntityUpdateInput
        =
        new(
            entityPluralName: "SomeEntities",
            entityKey: new StubEntityKey("Some key"),
            selectFields: new[] { "Some field name" },
            entityData: new()
            {
                Id = 1,
                Name = "Some name"
            });

    private static readonly DataverseEntityCreateIn<StubRequestJson> SomeDataverseEntityCreateInput
        =
        new(
            entityPluralName: "SomeEntities",
            selectFields: new[] { "Some field name" },
            entityData: new()
            {
                Id = 75,
                Name = "Some entity name"
            });

    private static readonly DataverseEntityDeleteIn SomeDataverseEntityDeleteInput
        =
        new(
            entityPluralName: "SomeEntitiesToDelete",
            entityKey: new StubEntityKey("Some entity key to delete"));

    private static readonly DataverseSearchIn SomeDataverseSearchInput
        =
        new("Some search text")
        {
            OrderBy = new[] { "field 1" },
            Top = 10,
            Skip = 5,
            ReturnTotalRecordCount = false,
            Filter = "Some filter",
            SearchMode = DataverseSearchMode.Any,
            SearchType = DataverseSearchType.Simple
        };

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