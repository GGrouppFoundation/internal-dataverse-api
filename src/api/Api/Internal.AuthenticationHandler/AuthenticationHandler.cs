using System.Net.Http;

namespace GGroupp.Infra;

internal sealed partial class AuthenticationHandler : DelegatingHandler
{
    private const string LoginMsOnlineServiceBaseUrl = "https://login.microsoftonline.com/";

    private readonly DataverseApiClientAuthOption option;

    internal AuthenticationHandler(HttpMessageHandler innerHandler, DataverseApiClientAuthOption option)
        : base(innerHandler)
        =>
        this.option = option;
}