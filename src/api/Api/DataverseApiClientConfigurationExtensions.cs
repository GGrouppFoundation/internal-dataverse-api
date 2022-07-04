using System;
using System.Diagnostics.CodeAnalysis;
using GGroupp.Infra;

namespace Microsoft.Extensions.Configuration;

public static class DataverseApiClientConfigurationExtensions
{
    internal const string DefaultSectionName = "Dataverse";

    public static DataverseApiClientOption GetDataverseApiClientOption(
        this IConfiguration configuration, [AllowNull] string sectionName = DefaultSectionName)
    {
        _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

        return configuration.InternalGetDataverseApiClientOption(sectionName.OrEmpty());
    }

    internal static DataverseApiClientOption InternalGetDataverseApiClientOption(
        this IConfiguration configuration, string sectionName)
        =>
        configuration.GetSection(sectionName).GetDataverseApiClientOption();

    private static DataverseApiClientOption GetDataverseApiClientOption(this IConfigurationSection section)
        =>
        new(
            serviceUrl: section["ServiceUrl"],
            authTenantId: section["AuthTenantId"],
            authClientId: section["AuthClientId"],
            authClientSecret: section["AuthClientSecret"]);
}