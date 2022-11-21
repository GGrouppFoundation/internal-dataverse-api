using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class AuthenticationHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var scopes = new[] { option.ServiceUrl + "/.default" };
        var tokenResult = await GetClientApplication(option).AcquireTokenForClient(scopes).ExecuteAsync(cancellationToken).ConfigureAwait(false);

        request.Headers.TryAddWithoutValidation("Authorization", tokenResult.CreateAuthorizationHeader());
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}