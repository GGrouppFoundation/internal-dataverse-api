using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class DataverseHttpApiTest
{
    [Theory]
    [MemberData(nameof(HttpApiTestDataSource.ChangeSetRequestTestData), MemberType = typeof(HttpApiTestDataSource))]
    internal static async Task SendChangeSetAsync_ExpectSendAsyncCalledOnce(
        Uri dataverseUri, DataverseChangeSetRequest request, StubHttpRequestData expetedHttpRequestData)
    {
        using var response = new HttpResponseMessage
        {
            Content = new MultipartContent("mixed", $"batch_{request.BatchId}")
        };

        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var httpApi = new DataverseHttpApi(messageHandler, dataverseUri);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await httpApi.SendChangeSetAsync(request, cancellationToken);

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
    [MemberData(nameof(HttpApiTestDataSource.UnauthorizedTestData), MemberType = typeof(HttpApiTestDataSource))]
    public static async Task SendChangeSetAsync_ResponseIsJsonUnauthorized_ExpectUnauthorizedFailure(
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

        var actual = await httpApi.SendChangeSetAsync(SomeChangeSetRequest, default);
        var expected = Failure.Create(DataverseFailureCode.Unauthorized, failureMessage);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HttpApiTestDataSource.FailureTestData), MemberType = typeof(HttpApiTestDataSource))]
    public static async Task SendChangeSetAsync_ResponseIsJsonFailure_ExpectFailure(
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

        var actual = await httpApi.SendChangeSetAsync(SomeChangeSetRequest, CancellationToken.None);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HttpApiTestDataSource.UnauthorizedTestData), MemberType = typeof(HttpApiTestDataSource))]
    public static async Task SendChangeSetAsync_ResponseContainsUnauthorized_ExpectUnauthorizedFailure(
        StringContent? responseContent, string failureMessage)
    {
        using var unauthorizedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Content = responseContent
        };

        using var response = new HttpResponseMessage
        {
            Content = new MultipartContent("mixed", "batch_f32ad236-f5a5-4d28-aa36-0051efac58ee")
            {
                new HttpMessageContent(unauthorizedResponse)
            }
        };

        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var httpApi = new DataverseHttpApi(messageHandler, SomeDataverseBaseUri);

        var actual = await httpApi.SendChangeSetAsync(SomeChangeSetRequest, default);
        var expected = Failure.Create(DataverseFailureCode.Unauthorized, failureMessage);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HttpApiTestDataSource.FailureTestData), MemberType = typeof(HttpApiTestDataSource))]
    public static async Task SendChangeSetAsync_ResponseContainsFailure_ExpectFailure(
        HttpStatusCode statusCode, StringContent? responseContent, Failure<DataverseFailureCode> expected)
    {
        using var successResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("Some text")
        };

        using var failureResponse = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = responseContent
        };

        using var response = new HttpResponseMessage
        {
            Content = new MultipartContent("mixed", "batch_12312")
            {
                new HttpMessageContent(successResponse),
                new HttpMessageContent(failureResponse)
            }
        };

        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var httpApi = new DataverseHttpApi(messageHandler, SomeDataverseBaseUri);

        var actual = await httpApi.SendChangeSetAsync(SomeChangeSetRequest, CancellationToken.None);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(HttpApiTestDataSource.ChangeSetSuccessTestData), MemberType = typeof(HttpApiTestDataSource))]
    internal static async Task SendChangeSetAsync_HttpResponseIsSuccess_ExpectSuccess(
        MultipartContent responseContent, DataverseChangeSetResponse expected)
    {
        using var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = responseContent
        };

        var mockProxyHandler = CreateMockProxyHandler(response);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var httpApi = new DataverseHttpApi(messageHandler, SomeDataverseBaseUri);

        var actual = await httpApi.SendChangeSetAsync(SomeChangeSetRequest, default);
        Assert.Equal(expected, actual);
    }
}