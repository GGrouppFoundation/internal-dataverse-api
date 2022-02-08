using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DeepEqual.Syntax;
using Moq;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public async Task SearchAsync_InputIsNull_ExpectArgumentNullException()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: false);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(
            () => dataverseApiClient.SearchAsync(null!, token).AsTask());

        Assert.Equal("input", ex.ParamName);
    }

    [Fact]
    public void SearchAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: true);

        var actualTask = dataverseApiClient.SearchAsync(SomeDataverseSearchInput, token);
        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetSearchInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task SearchAsync_CancellationTokenIsNotCanceled_ExpectGetRequest(
        Uri dataverseUri, DataverseSearchIn input, string expectedUrl, string expectedJson)
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, dataverseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.SearchAsync(input, token);

        mockProxyHandler.Verify(p => p.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        void Callback(HttpRequestMessage requestMessage)
        {
            Assert.Equal(HttpMethod.Post, requestMessage.Method);
            Assert.Equal(expectedUrl, requestMessage.RequestUri?.ToString(), ignoreCase: true);

            Assert.NotNull(requestMessage.Content);

            Assert.True(requestMessage.Content!.Headers.Contains("Prefer"));
            Assert.Equal("return=representation", requestMessage.Content?.Headers.GetValues("Prefer").First().ToString());

            Assert.Equal("application/json", requestMessage.Content?.Headers.ContentType?.MediaType);

            var actualJson = requestMessage.Content?.ReadAsStringAsync().Result;
            Assert.Equal(expectedJson, actualJson);
        }
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetFailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task SearchAsync_ResponseIsFailure_ExpectFailure(
        StringContent? responseContent, Failure<DataverseFailureCode> expected)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Conflict,
            Content = responseContent
        };
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var actual = await dataverseApiClient.SearchAsync(SomeDataverseSearchInput, CancellationToken.None);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetSearchOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task SearchAsync_ResponseIsSuccess_ExpectSuccess(
        StringContent? responseContent, DataverseSearchOut expected)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = responseContent
        };
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var input = SomeDataverseSearchInput;
        var actualResult = await dataverseApiClient.SearchAsync(input, default);

        Assert.True(actualResult.IsSuccess);
        var actual = actualResult.SuccessOrThrow();

        expected.ShouldDeepEqual(actual);
    }
}