using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public async Task CreateEntityAsync_InputIsNull_ExpectArgumentNullException()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: false);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(
            () => dataverseApiClient.CreateEntityAsync<StubRequestJson, Unit>(null!, token).AsTask());

        Assert.Equal("input", ex.ParamName);
    }

    [Fact]
    public void CreateEntityAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: true);

        var actualTask = dataverseApiClient.CreateEntityAsync<StubRequestJson, Unit>(SomeDataverseEntityCreateInput, token);
        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetEntityCreateInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task CreateEntityAsync_CancellationTokenIsNotCanceled_ExpectGetRequest(
        Uri dataverseUri, DataverseEntityCreateIn<StubRequestJson> input, string expectedUrl, string expectedJson)
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, dataverseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.CreateEntityAsync<StubRequestJson, StubResponseJson>(input, token);

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
    public async Task CreateEntityAsync_ResponseIsFailure_ExpectFailure(
        StringContent? responseContent, Failure<DataverseFailureCode> expected)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.TooManyRequests,
            Content = responseContent
        };
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var actual = await dataverseApiClient.CreateEntityAsync<StubRequestJson, Unit>(SomeDataverseEntityCreateInput, CancellationToken.None);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetUnitOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task CreateEntityAsync_UnitResponseIsSuccess_ExpectSuccess(
        StringContent? responseContent)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Created,
            Content = responseContent
        };
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var actual = await dataverseApiClient.CreateEntityAsync<StubRequestJson, Unit>(SomeDataverseEntityCreateInput, CancellationToken.None);
        var expected = new DataverseEntityCreateOut<Unit>(default);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetStubResponseJsonOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task CreateEntityAsync_ResponseJsonIsSuccess_ExpectSuccess(
        StringContent? responseContent, StubResponseJson? expectedValue)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = responseContent
        };
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var input = SomeDataverseEntityCreateInput;
        var actual = await dataverseApiClient.CreateEntityAsync<StubRequestJson, StubResponseJson>(input, default);

        var expected = new DataverseEntityCreateOut<StubResponseJson>(expectedValue);
        Assert.Equal(expected, actual);
    }
}