using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Identity.Client;

namespace GGroupp.Infra;

internal sealed partial class AuthenticationHandler : DelegatingHandler
{
    static AuthenticationHandler()
        =>
        ClientApplications = new();

    private static readonly Dictionary<DataverseApiClientAuthOption, IConfidentialClientApplication> ClientApplications;

    private const string LoginMsOnlineServiceBaseUrl = "https://login.microsoftonline.com/";

    private readonly DataverseApiClientAuthOption option;

    internal AuthenticationHandler(HttpMessageHandler innerHandler, DataverseApiClientAuthOption option)
        : base(innerHandler)
        =>
        this.option = option;

    private static IConfidentialClientApplication GetClientApplication(DataverseApiClientAuthOption option)
    {
        if (ClientApplications.TryGetValue(option, out var application))
        {
            return application;
        }

        var clientApplication = CreateClientApplication(option);
        ClientApplications[option] = clientApplication;

        return clientApplication;
    }

    private static IConfidentialClientApplication CreateClientApplication(DataverseApiClientAuthOption option)
        =>
        ConfidentialClientApplicationBuilder.Create(option.AuthClientId)
        .WithClientSecret(option.AuthClientSecret)
        .WithAuthority(LoginMsOnlineServiceBaseUrl + option.AuthTenantId)
        .Build();
}