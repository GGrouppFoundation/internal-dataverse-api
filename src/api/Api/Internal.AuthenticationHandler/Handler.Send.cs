using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace GarageGroup.Infra;

partial class AuthenticationHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = new TokenRequestContext(scopes: [option.ServiceUrl + "/.default"]);
        var token = await GetClientCredential(option).GetTokenAsync(context, cancellationToken).ConfigureAwait(false);

        request.Headers.Authorization = new(token.TokenType, token.Token);
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}