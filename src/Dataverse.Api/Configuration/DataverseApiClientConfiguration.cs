using System;

namespace GGroupp.Infra;

public sealed record class DataverseApiClientConfiguration
{
    public DataverseApiClientConfiguration(string serviceUrl, string authTenantId, string authClientId, string authClientSecret)
    {
        ServiceUrl = serviceUrl.OrEmpty();
        AuthTenantId = authTenantId.OrEmpty();
        AuthClientId = authClientId.OrEmpty();
        AuthClientSecret = authClientSecret.OrEmpty();
    }

    public string ServiceUrl { get; }

    public string AuthTenantId { get; }

    public string AuthClientId { get; }

    public string AuthClientSecret { get; }
}