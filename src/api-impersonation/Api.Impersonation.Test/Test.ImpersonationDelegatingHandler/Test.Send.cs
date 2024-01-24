using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Impersonation.Test;

partial class ImpersonationDelegatingHandlerTest
{
    [Fact]
    public static async Task SendAsync_RequestIsNull_ExpectArgumentNullException()
    {
        var mockCallerIdProvider = CreateMockCallerIdProvider(SomeCallerId);

        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var sourceHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var impersonationHandler = CreateImpersonationDelegatingHandler(sourceHandler, mockCallerIdProvider.Object);

        var httpClient = new HttpMessageInvoker(impersonationHandler);

        var token = new CancellationToken(canceled: false);
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => httpClient.SendAsync(null!, token));

        Assert.Equal("request", ex.ParamName);
    }

    [Fact]
    public static void SendAsync_CancellationTokenIsCanceled_ResultTaskIsCanceled()
    {
        var mockCallerIdProvider = CreateMockCallerIdProvider(SomeCallerId);

        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response);

        using var sourceHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var impersonationHandler = CreateImpersonationDelegatingHandler(sourceHandler, mockCallerIdProvider.Object);

        var httpClient = new HttpMessageInvoker(impersonationHandler);

        using var request = new HttpRequestMessage();
        var token = new CancellationToken(canceled: true);

        var resultTask = httpClient.SendAsync(request, token);
        Assert.True(resultTask.IsCanceled);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static async Task SendAsync_CancellationTokenIsNotCanceled_ExpectCallInnerHandlerWithCallerId(
        bool isSourceRequestWithCallerId)
    {
        const string callerId = "ac3a51a1-c8e1-4848-a556-ca75935d9e8c";
        var mockCallerIdProvider = CreateMockCallerIdProvider(Guid.Parse(callerId));

        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var sourceHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var impersonationHandler = CreateImpersonationDelegatingHandler(sourceHandler, mockCallerIdProvider.Object);

        var httpClient = new HttpMessageInvoker(impersonationHandler);

        using var request = new HttpRequestMessage();
        if (isSourceRequestWithCallerId)
        {
            request.Headers.Add(CallerIdHeaderName, "Some calleId");
        }

        var token = new CancellationToken(canceled: false);

        _ = await httpClient.SendAsync(request, token);
        mockProxyHandler.Verify(p => p.InvokeAsync(It.IsAny<HttpRequestMessage>(), token), Times.Once);
        
        static void Callback(HttpRequestMessage actualRequest)
        {
            var actualCallerId = actualRequest.Headers.GetValues(CallerIdHeaderName).First();
            Assert.Equal(callerId, actualCallerId);
        }
    }

    [Fact]
    public static async Task SendAsync_CancellationTokenIsNotCanceled_ExpectSourceResponse()
    {
        var mockCallerIdProvider = CreateMockCallerIdProvider(SomeCallerId);

        using var sourceResponse = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(sourceResponse);

        using var sourceHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var impersonationHandler = CreateImpersonationDelegatingHandler(sourceHandler, mockCallerIdProvider.Object);

        var httpClient = new HttpMessageInvoker(impersonationHandler);

        using var request = new HttpRequestMessage();
        var actual = await httpClient.SendAsync(request, default);

        Assert.Same(sourceResponse, actual);
    }
}