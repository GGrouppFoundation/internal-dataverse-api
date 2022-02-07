using System.Net.Http;

namespace GGroupp.Infra;

internal sealed partial class AuthenticationHandler : DelegatingHandler
{
    private const string LoginMsOnlineServiceBaseUrl = "https://login.microsoftonline.com/";

    private readonly DataverseApiClientConfiguration configuration;

    internal AuthenticationHandler(HttpMessageHandler innerHandler, DataverseApiClientConfiguration configuration)
        : base(innerHandler)
        =>
        this.configuration = configuration;
}