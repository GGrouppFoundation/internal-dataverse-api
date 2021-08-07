#nullable enable

using System;

namespace GGroupp.Infra
{
    public sealed record DataverseApiClientConfigurationJson : IDataverseApiClientConfiguration
    {
        public string? ServiceUrl { get; init; }

        public string? ApiVersion { get; init; }

        public string? AuthTenantId { get; init; }

        public string? AuthClientId { get; init; }

        public string? AuthClientSecret { get; init; }

        string IDataverseApiClientConfiguration.ServiceUrl => ServiceUrl.OrEmpty();

        string IDataverseApiClientConfiguration.ApiVersion => ApiVersion.OrEmpty();

        string IDataverseApiClientConfiguration.AuthTenantId => AuthTenantId.OrEmpty();

        string IDataverseApiClientConfiguration.AuthClientId => AuthClientId.OrEmpty();

        string IDataverseApiClientConfiguration.AuthClientSecret => AuthClientSecret.OrEmpty();
    }
}