using System;

namespace GGroupp.Infra;

public sealed record class DataverseApiClientAuthOption : DataverseApiClientOption
{
    public DataverseApiClientAuthOption(string serviceUrl, Guid authTenantId, string authClientId, string authClientSecret)
        : base(serviceUrl)
    {
        AuthTenantId = authTenantId;
        AuthClientId = authClientId.OrEmpty();
        AuthClientSecret = authClientSecret.OrEmpty();
    }

    public Guid AuthTenantId { get; }

    public string AuthClientId { get; }

    public string AuthClientSecret { get; }
}