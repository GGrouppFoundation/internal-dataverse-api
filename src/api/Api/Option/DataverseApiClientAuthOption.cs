using System;

namespace GGroupp.Infra;

public sealed record class DataverseApiClientAuthOption : DataverseApiClientOption
{
    public DataverseApiClientAuthOption(string serviceUrl, string authTenantId, string authClientId, string authClientSecret)
        : base(serviceUrl)
    {
        AuthTenantId = authTenantId.OrEmpty();
        AuthClientId = authClientId.OrEmpty();
        AuthClientSecret = authClientSecret.OrEmpty();
    }

    public string AuthTenantId { get; }

    public string AuthClientId { get; }

    public string AuthClientSecret { get; }
}