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
    public async Task UpdateEntityAsync_InputIsNull_ExpectArgumentNullException()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: false);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(
            () => dataverseApiClient.UpdateEntityAsync<StubRequestJson, Unit>(null!, token).AsTask());

        Assert.Equal("input", ex.ParamName);
    }

    [Fact]
    public void UpdateEntityAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: true);

        var actualTask = dataverseApiClient.UpdateEntityAsync<StubRequestJson, Unit>(SomeDataverseEntityUpdateInput, token);
        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetEntityUpdateInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task UpdateEntityAsync_CancellationTokenIsNotCanceled_ExpectGetRequest(
        Uri dataverseUri, DataverseEntityUpdateIn<StubRequestJson> input, string expectedUrl, string expectedJson)
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, dataverseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.UpdateEntityAsync<StubRequestJson, StubResponseJson>(input, token);

        mockProxyHandler.Verify(p => p.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        void Callback(HttpRequestMessage requestMessage)
        {
            Assert.Equal(HttpMethod.Patch, requestMessage.Method);
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
    public async Task UpdateEntityAsync_ResponseIsFailure_ExpectFailure(
        StringContent? responseContent, Failure<DataverseFailureCode> expected)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
            Content = responseContent
        };
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var actual = await dataverseApiClient.UpdateEntityAsync<StubRequestJson, Unit>(SomeDataverseEntityUpdateInput, CancellationToken.None);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetUnitOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task UpdateEntityAsync_UnitResponseIsSuccess_ExpectSuccess(
        StringContent? responseContent)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = responseContent
        };
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var actual = await dataverseApiClient.UpdateEntityAsync<StubRequestJson, Unit>(SomeDataverseEntityUpdateInput, CancellationToken.None);
        var expected = new DataverseEntityUpdateOut<Unit>(default);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetStubResponseJsonOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task UpdateEntityAsync_ResponseJsonIsSuccess_ExpectSuccess(
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

        var input = SomeDataverseEntityUpdateInput;
        var actual = await dataverseApiClient.UpdateEntityAsync<StubRequestJson, StubResponseJson>(input, default);

        var expected = new DataverseEntityUpdateOut<StubResponseJson>(expectedValue);
        Assert.Equal(expected, actual);
    }
}