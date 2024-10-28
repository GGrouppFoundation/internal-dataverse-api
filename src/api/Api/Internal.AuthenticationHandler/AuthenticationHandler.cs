using System.Collections.Concurrent;
using System.Net.Http;
using Azure.Identity;

namespace GarageGroup.Infra;

internal sealed partial class AuthenticationHandler : DelegatingHandler
{
    static AuthenticationHandler()
        =>
        ClientCredentials = new();

    private static readonly ConcurrentDictionary<DataverseApiClientAuthOption, ClientSecretCredential> ClientCredentials;

    private readonly DataverseApiClientAuthOption option;

    internal AuthenticationHandler(HttpMessageHandler innerHandler, DataverseApiClientAuthOption option)
        : base(innerHandler)
        =>
        this.option = option;

    private static ClientSecretCredential GetClientCredential(DataverseApiClientAuthOption option)
    {
        return ClientCredentials.GetOrAdd(option, CreateClientCredential);

        static ClientSecretCredential CreateClientCredential(DataverseApiClientAuthOption option)
            =>
            new(
                tenantId: option.AuthTenantId.ToString(),
                clientId: option.AuthClientId,
                clientSecret: option.AuthClientSecret);
    }
}