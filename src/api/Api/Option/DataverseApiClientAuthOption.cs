using System;

namespace GarageGroup.Infra;

public sealed record class DataverseApiClientAuthOption : DataverseApiClientOption
{
    public DataverseApiClientAuthOption(
        string serviceUrl, 
        Guid authTenantId, 
        string authClientId, 
        string authClientSecret, 
        TimeSpan? httpTimeOut = null)
        : base(serviceUrl, httpTimeOut)
    {
        AuthTenantId = authTenantId;
        AuthClientId = authClientId.OrEmpty();
        AuthClientSecret = authClientSecret.OrEmpty();
    }

    public Guid AuthTenantId { get; }

    public string AuthClientId { get; }

    public string AuthClientSecret { get; }
}