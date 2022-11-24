using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseHttpApiTest
{
    [Theory]
    [MemberData(nameof(HttpApiTestDataSource.GetRequestTestData), MemberType = typeof(HttpApiTestDataSource))]
    internal async Task InvokeAsync_ExpectSendAsyncCalledOnce(
        Uri dataverseUri, DataverseHttpRequest<StubRequestJson> request, StubHttpRequestData expetedHttpRequestData)
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var httpApi = new DataverseHttpApi(messageHandler, dataverseUri);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await httpApi.InvokeAsync<StubRequestJson, StubResponseJson>(request, cancellationToken);

        mockProxyHandler.Verify(p => p.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        void Callback(HttpRequestMessage requestMessage)
        {
            Assert.Equal(expetedHttpRequestData.Method, requestMessage.Method);

            var actualRequestUrl = requestMessage.RequestUri?.ToString();
            Assert.Equal(expetedHttpRequestData.RequestUrl, actualRequestUrl, ignoreCase: true);

            var actualContent = requestMessage.Content?.ReadAsStringAsync().Result;
            Assert.Equal(expetedHttpRequestData.Content, actualContent);

            var actualHeaderValues = ReadHeaderValues(requestMessage);
            Assert.Equal(expetedHttpRequestData.Headers.AsEnumerable(), actualHeaderValues.AsEnumerable());
        }
    }

    [Theory]
    [MemberData(nameof(HttpApiTestDataSource.GetUnauthorizedTestData), MemberType = typeof(HttpApiTestDataSource))]
    public async Task InvokeAsync_ResponseIsUnauthorized_ExpectUnauthorizedFailure(
        StringContent? responseContent, string failureMessage)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Content = responseContent
        };

        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var httpApi = new DataverseHttpApi(messageHandler, SomeDataverseBaseUri);

        var request = CreateRequest(new StubRequestJson());

        var actual = await httpApi.InvokeAsync<StubRequestJson, StubRequestJson>(request, default);
        var expected = Failure.Create(DataverseFailureCode.Unauthorized, failureMessage);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HttpApiTestDataSource.GetFailuretTestData), MemberType = typeof(HttpApiTestDataSource))]
    public async Task InvokeAsync_ResponseIsFailure_ExpectFailure(
        HttpStatusCode statusCode, StringContent? responseContent, Failure<DataverseFailureCode> expected)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = responseContent
        };

        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var httpApi = new DataverseHttpApi(messageHandler, SomeDataverseBaseUri);

        var request = CreateRequest<Unit>(default);
        var actual = await httpApi.InvokeAsync<Unit, StubRequestJson>(request, CancellationToken.None);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HttpApiTestDataSource.GetUnitSuccessTestData), MemberType = typeof(HttpApiTestDataSource))]
    public async Task InvokeAsync_HttpResponseIsSuccessWhenUnitIsOutType_ExpectSuccess(
        StringContent? responseContent)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Created,
            Content = responseContent
        };

        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var httpApi = new DataverseHttpApi(messageHandler, SomeDataverseBaseUri);

        var request = CreateRequest("Some value");

        var actual = await httpApi.InvokeAsync<string, Unit>(request, CancellationToken.None);
        var expected = Result.Success<Unit>(default);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HttpApiTestDataSource.GetSuccessTestData), MemberType = typeof(HttpApiTestDataSource))]
    internal async Task GetEntityAsync_HttpResponseIsSuccess_ExpectSuccess(
        StringContent? responseContent, StubResponseJson? expected)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = responseContent
        };

        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var httpApi = new DataverseHttpApi(messageHandler, SomeDataverseBaseUri);

        var input = new StubRequestJson
        {
            Id = 1
        };

        var request = CreateRequest(input);

        var actual = await httpApi.InvokeAsync<StubRequestJson, StubResponseJson>(request, default);
        Assert.Equal(expected, actual);
    }
}