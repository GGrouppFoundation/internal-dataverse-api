using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public async Task Impersonate_Then_CreateEntityAsync_ExpectHeadersContainCallerId()
    {
        using var response = new HttpResponseMessage();
        const string callerIdValue = "cf6678d2-2963-4f14-8dff-21c956ae9695";

        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var callerId = Guid.Parse(callerIdValue);
        var impersonatedApiClient = dataverseApiClient.Impersonate(callerId);

        var token = new CancellationToken(canceled: false);
        _ = await impersonatedApiClient.CreateEntityAsync<StubRequestJson, StubResponseJson>(SomeDataverseEntityCreateInput, token);

        static void Callback(HttpRequestMessage actualRequest)
        {
            var actualCallerIdValue = actualRequest.Headers.GetValues(CallerIdHeaderName).First();
            Assert.Equal(callerIdValue, actualCallerIdValue);
        }
    }

    [Fact]
    public async Task NotImpersonate_Then_CreateEntityAsync_ExpectHeadersDoNotContainCallerId()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, DoesNotContainCallerIdHeader);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.CreateEntityAsync<StubRequestJson, StubResponseJson>(SomeDataverseEntityCreateInput, token);
    }

    [Fact]
    public async Task Impersonate_Then_DeleteEntityAsync_ExpectHeadersContainCallerId()
    {
        using var response = new HttpResponseMessage();
        const string callerIdValue = "91526fc6-1491-4ee9-8b7a-a4ed536de862";

        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var callerId = Guid.Parse(callerIdValue);
        var impersonatedApiClient = dataverseApiClient.Impersonate(callerId);

        var token = new CancellationToken(canceled: false);
        _ = await impersonatedApiClient.DeleteEntityAsync(SomeDataverseEntityDeleteInput, token);

        static void Callback(HttpRequestMessage actualRequest)
        {
            var actualCallerIdValue = actualRequest.Headers.GetValues(CallerIdHeaderName).First();
            Assert.Equal(callerIdValue, actualCallerIdValue);
        }
    }

    [Fact]
    public async Task NotImpersonate_Then_DeleteEntityAsync_ExpectHeadersDoNotContainCallerId()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, DoesNotContainCallerIdHeader);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.DeleteEntityAsync(SomeDataverseEntityDeleteInput, token);
    }

    [Fact]
    public async Task Impersonate_Then_GetEntityAsync_ExpectHeadersContainCallerId()
    {
        using var response = new HttpResponseMessage();
        const string callerIdValue = "18945ff7-9433-4e74-a403-abd6db25ef27";

        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var callerId = Guid.Parse(callerIdValue);
        var impersonatedApiClient = dataverseApiClient.Impersonate(callerId);

        var token = new CancellationToken(canceled: false);
        _ = await impersonatedApiClient.GetEntityAsync<StubResponseJson>(SomeDataverseEntityGetInput, token);

        static void Callback(HttpRequestMessage actualRequest)
        {
            var actualCallerIdValue = actualRequest.Headers.GetValues(CallerIdHeaderName).First();
            Assert.Equal(callerIdValue, actualCallerIdValue);
        }
    }

    [Fact]
    public async Task NotImpersonate_Then_GetEntityAsync_ExpectHeadersDoNotContainCallerId()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, DoesNotContainCallerIdHeader);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.GetEntityAsync<StubResponseJson>(SomeDataverseEntityGetInput, token);
    }

    [Fact]
    public async Task Impersonate_Then_GetEntitySetAsync_ExpectHeadersContainCallerId()
    {
        using var response = new HttpResponseMessage();
        const string callerIdValue = "d44c6578-1f2e-4edd-8897-77aaf8bd524a";

        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var callerId = Guid.Parse(callerIdValue);
        var impersonatedApiClient = dataverseApiClient.Impersonate(callerId);

        var token = new CancellationToken(canceled: false);
        _ = await impersonatedApiClient.GetEntitySetAsync<StubResponseJson>(SomeDataverseEntitySetGetInput, token);

        static void Callback(HttpRequestMessage actualRequest)
        {
            var actualCallerIdValue = actualRequest.Headers.GetValues(CallerIdHeaderName).First();
            Assert.Equal(callerIdValue, actualCallerIdValue);
        }
    }

    [Fact]
    public async Task NotImpersonate_Then_GetEntitySetAsync_ExpectHeadersDoNotContainCallerId()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, DoesNotContainCallerIdHeader);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.GetEntitySetAsync<StubResponseJson>(SomeDataverseEntitySetGetInput, token);
    }

    [Fact]
    public async Task Impersonate_Then_SearchAsync_ExpectHeadersContainCallerId()
    {
        using var response = new HttpResponseMessage();
        const string callerIdValue = "aa087335-0897-4d6e-82cb-0f07cb6fc2f4";

        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var callerId = Guid.Parse(callerIdValue);
        var impersonatedApiClient = dataverseApiClient.Impersonate(callerId);

        var token = new CancellationToken(canceled: false);
        _ = await impersonatedApiClient.SearchAsync(SomeDataverseSearchInput, token);

        static void Callback(HttpRequestMessage actualRequest)
        {
            var actualCallerIdValue = actualRequest.Headers.GetValues(CallerIdHeaderName).First();
            Assert.Equal(callerIdValue, actualCallerIdValue);
        }
    }

    [Fact]
    public async Task NotImpersonate_Then_SearchAsync_ExpectHeadersDoNotContainCallerId()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, DoesNotContainCallerIdHeader);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.SearchAsync(SomeDataverseSearchInput, token);
    }

    [Fact]
    public async Task Impersonate_Then_UpdateEntityAsync_ExpectHeadersContainCallerId()
    {
        using var response = new HttpResponseMessage();
        const string callerIdValue = "9fdea890-f164-47c1-bb51-d3865229fa9b";

        var mockProxyHandler = CreateMockProxyHandler(response, Callback);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var callerId = Guid.Parse(callerIdValue);
        var impersonatedApiClient = dataverseApiClient.Impersonate(callerId);

        var token = new CancellationToken(canceled: false);
        _ = await impersonatedApiClient.UpdateEntityAsync<StubRequestJson, Unit>(SomeDataverseEntityUpdateInput, token);

        static void Callback(HttpRequestMessage actualRequest)
        {
            var actualCallerIdValue = actualRequest.Headers.GetValues(CallerIdHeaderName).First();
            Assert.Equal(callerIdValue, actualCallerIdValue);
        }
    }

    [Fact]
    public async Task NotImpersonate_Then_UpdateEntityAsync_ExpectHeadersDoNotContainCallerId()
    {
        using var response = new HttpResponseMessage();
        var mockProxyHandler = CreateMockProxyHandler(response, DoesNotContainCallerIdHeader);

        using var messageHandler = new StubHttpMessageHandler(mockProxyHandler.Object);
        var dataverseApiClient = CreateDataverseApiClient(messageHandler, SomeDataverseBaseUri);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.UpdateEntityAsync<StubRequestJson, Unit>(SomeDataverseEntityUpdateInput, token);
    }
}