using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GGroupp.Infra;

partial class AuthenticationHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authContext = new AuthenticationContext(LoginMsOnlineServiceBaseUrl + option.AuthTenantId);
        var credential = new ClientCredential(option.AuthClientId, option.AuthClientSecret);

        var authTokenResult = await authContext.AcquireTokenAsync(option.ServiceUrl, credential).ConfigureAwait(false);
        request.Headers.TryAddWithoutValidation("Authorization", authTokenResult.CreateAuthorizationHeader());

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}