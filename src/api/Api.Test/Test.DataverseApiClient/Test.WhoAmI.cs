using System;
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
    public void WhoAmIAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: true);
        var actualTask = dataverseApiClient.WhoAmIAsync(default, token);

        Assert.True(actualTask.IsCanceled);
    }

    [Fact]
    public async Task WhoAmIAsync_CancellationTokenIsNotCanceled_ExpectGetRequest()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseUri = new Uri("https://some.dynamics.com/", UriKind.Absolute);

        var dataverseApiClient = CreateDataverseApiClient(messageHandler, dataverseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.WhoAmIAsync(default, token);

        mockProxyHandler.Verify(p => p.InvokeAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()), Times.Once);

        void Callback(HttpRequestMessage requestMessage)
        {
            Assert.Equal(HttpMethod.Delete, requestMessage.Method);

            const string expectedUrl = "https://some.dynamics.com/api/data/v9.2/WhoAmI";
            Assert.Equal(expectedUrl, requestMessage.RequestUri?.ToString(), ignoreCase: true);
        }
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetUnauthorizedOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task WhoAmIAsync_ResponseIsUnauthorized_ExpectUnauthorizedFailure(
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

        var actual = await dataverseApiClient.WhoAmIAsync(default, CancellationToken.None);
        var expected = Failure.Create(DataverseFailureCode.Unauthorized, failureMessage);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetFailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task WhoAmIAsync_ResponseIsFailure_ExpectFailure(
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

        var actual = await dataverseApiClient.WhoAmIAsync(default, CancellationToken.None);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetWhoAmIOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task WhoAmIAsync_ResponseIsSuccess_ExpectSuccess(
        StringContent? responseContent, DataverseWhoAmIOut expected)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = responseContent
        };

        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var actual = await dataverseApiClient.WhoAmIAsync(default, CancellationToken.None);
        Assert.Equal(expected, actual);
    }
}