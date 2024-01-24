using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Dataverse.Api.Impersonation.Test;

internal sealed class StubHttpMessageHandler(IAsyncFunc<HttpRequestMessage, HttpResponseMessage> proxyHandler) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        =>
        proxyHandler.InvokeAsync(request, cancellationToken);
}