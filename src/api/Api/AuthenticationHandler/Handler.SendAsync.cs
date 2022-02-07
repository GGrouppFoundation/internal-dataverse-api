using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GGroupp.Infra;

partial class AuthenticationHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authContext = new AuthenticationContext(LoginMsOnlineServiceBaseUrl + configuration.AuthTenantId);
        var credential = new ClientCredential(configuration.AuthClientId, configuration.AuthClientSecret);

        var authTokenResult = await authContext.AcquireTokenAsync(configuration.ServiceUrl, credential).ConfigureAwait(false);
        request.Headers.TryAddWithoutValidation("Authorization", authTokenResult.CreateAuthorizationHeader());

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}