using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class StandardAzureCredentialHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Headers.Authorization is not null || request.RequestUri is null)
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        var context = CreateRequestContext(request.RequestUri);

        var token = await LazyCredential.Value.GetTokenAsync(context, cancellationToken).ConfigureAwait(false);
        request.Headers.Authorization = new(AuthorizationScheme, token.Token);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}