#nullable enable

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GGroupp
{
    internal static class HttpClientFactory
    {
        private const string LoginMsOnlineServiceBaseUrl = "https://login.microsoftonline.com/";

        public static async Task<HttpClient> CreateHttpClientAsync(
            IDataverseApiClientConfiguration clientConfiguration, HttpMessageHandler messageHandler)
        {
            var authContext = CreateAuthenticationContext(clientConfiguration.AuthTenantId);
            var credential = new ClientCredential(clientConfiguration.AuthClientId, clientConfiguration.AuthClientSecret);

            var client = new HttpClient(messageHandler, disposeHandler: false)
            {
                BaseAddress = new($"{clientConfiguration.ServiceUrl}/api/data/v{clientConfiguration.ApiVersion}/")
            };

            var authTokenResult = await authContext.AcquireTokenAsync(clientConfiguration.ServiceUrl, credential).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new(authTokenResult.AccessTokenType, authTokenResult.AccessToken);

            return client;
        }

        private static AuthenticationContext CreateAuthenticationContext(string tenantId)
            =>
            new(LoginMsOnlineServiceBaseUrl + tenantId);
    }
}