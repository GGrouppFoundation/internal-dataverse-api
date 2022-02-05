using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class ImpersonationDelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<HttpResponseMessage>(cancellationToken);
        }

        return InnerSendAsync(request, cancellationToken);
    }

    private async Task<HttpResponseMessage> InnerSendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if(request.Headers.Contains(CallerIdHeaderName))
        {
            request.Headers.Remove(CallerIdHeaderName);
        }

        var callerId = await callerIdProvider.InvokeAsync(cancellationToken).ConfigureAwait(false);
        request.Headers.Add(CallerIdHeaderName, callerId.ToString("D", CultureInfo.InvariantCulture));

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}