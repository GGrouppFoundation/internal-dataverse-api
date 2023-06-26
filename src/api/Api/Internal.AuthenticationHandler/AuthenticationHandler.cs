using System.Collections.Concurrent;
using System.Net.Http;
using Microsoft.Identity.Client;

namespace GarageGroup.Infra;

internal sealed partial class AuthenticationHandler : DelegatingHandler
{
    static AuthenticationHandler()
        =>
        ClientApplications = new();

    private static readonly ConcurrentDictionary<DataverseApiClientAuthOption, IConfidentialClientApplication> ClientApplications;

    private const string LoginMsOnlineServiceBaseUrl = "https://login.microsoftonline.com/";

    private readonly DataverseApiClientAuthOption option;

    internal AuthenticationHandler(HttpMessageHandler innerHandler, DataverseApiClientAuthOption option)
        : base(innerHandler)
        =>
        this.option = option;

    private static IConfidentialClientApplication GetClientApplication(DataverseApiClientAuthOption option)
    {
        return ClientApplications.GetOrAdd(option, CreateClientApplication);

        static IConfidentialClientApplication CreateClientApplication(DataverseApiClientAuthOption option)
            =>
            ConfidentialClientApplicationBuilder.Create(option.AuthClientId)
            .WithClientSecret(option.AuthClientSecret)
            .WithAuthority(LoginMsOnlineServiceBaseUrl + option.AuthTenantId)
            .Build();
    }
}