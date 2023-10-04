using System;
using System.Diagnostics.CodeAnalysis;
using GarageGroup.Infra;

namespace Microsoft.Extensions.Configuration;

public static class DataverseApiClientConfigurationExtensions
{
    internal const string DefaultSectionName = "Dataverse";

    private const string ServiceUrlKey = "ServiceUrl";

    private const string AuthTenantIdKey = "AuthTenantId";

    private const string AuthClientIdKey = "AuthClientId";

    private const string AuthClientSecretKey = "AuthClientSecret";

    private const string HttpTimeOutKey = "HttpTimeOut";

    public static DataverseApiClientOption GetDataverseApiClientOption(
        this IConfiguration configuration, [AllowNull] string sectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return configuration.InternalGetDataverseApiClientOption(sectionName.OrEmpty());
    }

    public static DataverseApiClientAuthOption GetDataverseApiClientAuthOption(
        this IConfiguration configuration, [AllowNull] string sectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return configuration.InternalGetDataverseApiClientAuthOption(sectionName.OrEmpty());
    }

    internal static DataverseApiClientOption InternalGetDataverseApiClientOption(
        this IConfiguration configuration, string sectionName)
        =>
        string.IsNullOrEmpty(configuration[AuthClientSecretKey]) switch
        {
            false => configuration.GetSection(sectionName).GetDataverseApiClientAuthOption(),
            _ => configuration.GetSection(sectionName).GetDataverseApiClientOption()
        };

    internal static DataverseApiClientAuthOption InternalGetDataverseApiClientAuthOption(
        this IConfiguration configuration, string sectionName)
        =>
        configuration.GetSection(sectionName).GetDataverseApiClientAuthOption();

    private static DataverseApiClientOption GetDataverseApiClientOption(this IConfigurationSection section)
        =>
        new(
            serviceUrl: section[ServiceUrlKey].OrEmpty(),
            httpTimeOut:  GetTimeSpanOrDefault(section, HttpTimeOutKey));

    private static DataverseApiClientAuthOption GetDataverseApiClientAuthOption(this IConfigurationSection section)
        =>
        new(
            serviceUrl: section[ServiceUrlKey].OrEmpty(),
            authTenantId: section.GetGuid(AuthTenantIdKey),
            authClientId: section[AuthClientIdKey].OrEmpty(),
            authClientSecret: section[AuthClientSecretKey].OrEmpty(),
            httpTimeOut: GetTimeSpanOrDefault(section, HttpTimeOutKey));

    private static Guid GetGuid(this IConfiguration configuration, string key)
    {
        var value = configuration[key];

        if (string.IsNullOrEmpty(value))
        {
            return default;
        }

        return Guid.Parse(value);
    }

    private static TimeSpan? GetTimeSpanOrDefault(this IConfiguration configuration, string key)
    {
        var value = configuration[key];

        if (string.IsNullOrEmpty(value))
        {
            return default;
        }

        return TimeSpan.Parse(value);
    }
}