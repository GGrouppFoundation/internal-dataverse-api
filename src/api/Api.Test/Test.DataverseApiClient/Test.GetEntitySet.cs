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
    public async Task GetEntitySetAsync_InputIsNull_ExpectArgumentNullException()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: false);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerGetEntitySetAsync);

        Assert.Equal("input", ex.ParamName);

        Task InnerGetEntitySetAsync()
            =>
            dataverseApiClient.GetEntitySetAsync<Unit>(null!, token).AsTask();
    }

    [Fact]
    public void GetEntitySetAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var input = SomeDataverseEntitySetGetInput;
        var token = new CancellationToken(canceled: true);

        var actualTask = dataverseApiClient.GetEntitySetAsync<StubResponseJson>(input, token);
        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetEntitySetGetInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task GetEntitySetAsync_CancellationTokenIsNotCanceled_ExpectGetRequest(
        Uri dataverseUri, DataverseEntitySetGetIn input, string expectedUrl, string? expectedPreferHeaderValue)
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, dataverseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.GetEntitySetAsync<StubResponseJson>(input, token);

        mockProxyHandler.Verify(p => p.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        void Callback(HttpRequestMessage requestMessage)
        {
            Assert.Equal(HttpMethod.Get, requestMessage.Method);
            Assert.Equal(expectedUrl, requestMessage.RequestUri?.ToString(), ignoreCase: true);

            var actualContainsPreferHeaderValue = requestMessage.Headers.Contains("Prefer");
            if (expectedPreferHeaderValue is not null)
            {
                Assert.True(actualContainsPreferHeaderValue);
                Assert.Equal(expectedPreferHeaderValue, requestMessage.Headers.GetValues("Prefer").First().ToString());
            }
            else
            {
                Assert.False(actualContainsPreferHeaderValue);
            }
        }
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetUnauthorizedOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task GetEntitySetAsync_ResponseIsUnauthorized_ExpectUnauthorizedFailure(
        StringContent? responseContent, string failureMessage)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Content = responseContent
        };
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var actual = await dataverseApiClient.GetEntitySetAsync<StubRequestJson>(SomeDataverseEntitySetGetInput, CancellationToken.None);
        var expected = Failure.Create(DataverseFailureCode.Unauthorized, failureMessage);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetFailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task GetEntitySetAsync_ResponseIsFailure_ExpectFailure(
        HttpStatusCode statusCode, StringContent? responseContent, Failure<DataverseFailureCode> expected)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = responseContent
        };
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var input = SomeDataverseEntitySetGetInput;
        var actual = await dataverseApiClient.GetEntitySetAsync<StubResponseJson>(input, CancellationToken.None);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetStubResponseJsonSetOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task GetEntitySetAsync_ResponseJsonIsSuccess_ExpectSuccess(
        StringContent? responseContent, DataverseEntitySetGetOut<StubResponseJson> expected)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = responseContent
        };

        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var actual = await dataverseApiClient.GetEntitySetAsync<StubResponseJson>(SomeDataverseEntitySetGetInput, CancellationToken.None);
        Assert.Equal(expected, actual);
    }
}