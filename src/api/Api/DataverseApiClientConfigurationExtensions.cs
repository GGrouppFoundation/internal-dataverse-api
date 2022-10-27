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

    public static DataverseApiClientAuthOption GetDataverseApiClientAuthOption(
        this IConfiguration configuration, [AllowNull] string sectionName = DefaultSectionName)
    {
        _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

        return configuration.InternalGetDataverseApiClientAuthOption(sectionName.OrEmpty());
    }

    internal static DataverseApiClientOption InternalGetDataverseApiClientOption(
        this IConfiguration configuration, string sectionName)
        =>
        configuration.GetSection(sectionName).GetDataverseApiClientOption();

    internal static DataverseApiClientAuthOption InternalGetDataverseApiClientAuthOption(
        this IConfiguration configuration, string sectionName)
        =>
        configuration.GetSection(sectionName).GetDataverseApiClientAuthOption();

    private static DataverseApiClientOption GetDataverseApiClientOption(this IConfigurationSection section)
        =>
        new(
            serviceUrl: section["ServiceUrl"]);

    private static DataverseApiClientAuthOption GetDataverseApiClientAuthOption(this IConfigurationSection section)
        =>
        new(
            serviceUrl: section["ServiceUrl"],
            authTenantId: section["AuthTenantId"],
            authClientId: section["AuthClientId"],
            authClientSecret: section["AuthClientSecret"]);
}