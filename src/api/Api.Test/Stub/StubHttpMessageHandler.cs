using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Dataverse.Api.Test;

internal sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly IAsyncFunc<HttpRequestMessage, HttpResponseMessage> proxyHandler;

    public StubHttpMessageHandler(IAsyncFunc<HttpRequestMessage, HttpResponseMessage> proxyHandler)
        =>
        this.proxyHandler = proxyHandler;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        =>
        proxyHandler.InvokeAsync(request, cancellationToken);
}