using System.Net.Http;

namespace GGroupp.Infra;

internal sealed partial class AuthenticationHandler : DelegatingHandler
{
    private const string LoginMsOnlineServiceBaseUrl = "https://login.microsoftonline.com/";

    private readonly DataverseApiClientOption option;

    internal AuthenticationHandler(HttpMessageHandler innerHandler, DataverseApiClientOption option)
        : base(innerHandler)
        =>
        this.option = option;
}