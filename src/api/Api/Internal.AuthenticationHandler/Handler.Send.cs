using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class AuthenticationHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string[] scopes = [option.ServiceUrl + "/.default"];
        var token = await GetClientApplication(option).AcquireTokenForClient(scopes).ExecuteAsync(cancellationToken).ConfigureAwait(false);

        request.Headers.Authorization = AuthenticationHeaderValue.Parse(token.CreateAuthorizationHeader());
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}