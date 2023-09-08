using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Moq;

namespace GarageGroup.Infra.Dataverse.Api.Test;

public static partial class DataverseHttpApiTest
{
    private static readonly Uri SomeDataverseBaseUri
        =
        new("https://some.crm4.dynamics.com/", UriKind.Absolute);

    private static readonly DataverseJsonRequest SomeJsonRequest
        =
        new(
            verb: DataverseHttpVerb.Post,
            url: "/data?$select=field1,field2&$filter=a eq 1",
            headers: new(
                new("first", "one "),
                new("second", "two")),
            content: new("{\"id\": 1, \"name\": \"Some name\"}"));

    private static readonly DataverseChangeSetRequest SomeChangeSetRequest
        =
        new(
            url: "/api/data/v9.2/$batch",
            batchId: Guid.Parse("ef3a07fb-4036-4f9c-b79b-441ec1a53e54"),
            changeSetId: Guid.Parse("4d177bb6-49bc-41f5-b3cf-158b3848f7ca"),
            headers: new(
                new("first", "one"),
                new("second", "two")),
            requests: SomeJsonRequest.AsFlatArray());

    private static Mock<IAsyncFunc<HttpRequestMessage, HttpResponseMessage>> CreateMockProxyHandler(
        HttpResponseMessage responseMessage, Action<HttpRequestMessage>? callback = default)
    {
        var mock = new Mock<IAsyncFunc<HttpRequestMessage, HttpResponseMessage>>();

        var m = mock
            .Setup(p => p.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseMessage);

        if (callback is not null)
        {
            _ = m.Callback<HttpRequestMessage, CancellationToken>(
                (r, _) => callback.Invoke(r));
        }

        return mock;
    }

    private static Mock<IAsyncFunc<HttpRequestMessage, HttpResponseMessage>> CreateMockProxyHandler(
        Exception exception)
    {
        var mock = new Mock<IAsyncFunc<HttpRequestMessage, HttpResponseMessage>>();

        _ = mock
            .Setup(p => p.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        return mock;
    }

    private static FlatArray<KeyValuePair<string, string>> ReadHeaderValues(HttpRequestMessage requestMessage)
    {
        var headers = requestMessage.Headers.Select(MapValue);
        if (requestMessage.Content is null)
        {
            return headers.ToFlatArray();
        }

        return headers.Concat(requestMessage.Content.Headers.Select(MapValue)).ToFlatArray();

        static KeyValuePair<string, string> MapValue(KeyValuePair<string, IEnumerable<string>> source)
            =>
            new(
                source.Key, string.Join(',', source.Value));
    }
}