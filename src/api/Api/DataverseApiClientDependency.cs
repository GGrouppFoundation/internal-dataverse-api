using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;
using static Microsoft.Extensions.Configuration.DataverseApiClientConfigurationExtensions;

[assembly: InternalsVisibleTo("GarageGroup.Infra.Dataverse.Api.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GarageGroup.Infra;

public static class DataverseApiClientDependency
{
    public static Dependency<IDataverseApiClient> UseDataverseApiClient(
        this Dependency<HttpMessageHandler, DataverseApiClientAuthOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Fold<IDataverseApiClient>(CreateApiClient);
    }

    public static Dependency<IDataverseApiClient> UseDataverseApiClient(
        this Dependency<HttpMessageHandler> dependency, Func<IServiceProvider, DataverseApiClientAuthOption> optionResolver)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(optionResolver);

        return dependency.With(optionResolver).Fold<IDataverseApiClient>(CreateApiClient);
    }

    public static Dependency<IDataverseApiClient> UseDataverseApiClient(
        this Dependency<HttpMessageHandler> dependency, [AllowNull] string sectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Map<IDataverseApiClient>(CreateClient);

        DataverseApiClient CreateClient(IServiceProvider serviceProvider, HttpMessageHandler httpMessageHandler)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(httpMessageHandler);

            var option = serviceProvider.GetServiceOrThrow<IConfiguration>().InternalGetDataverseApiClientAuthOption(sectionName.OrEmpty());
            return InnerCreateApiClient(httpMessageHandler, option);
        }
    }

    private static DataverseApiClient CreateApiClient(HttpMessageHandler httpMessageHandler, DataverseApiClientAuthOption option)
    {
        ArgumentNullException.ThrowIfNull(httpMessageHandler);
        ArgumentNullException.ThrowIfNull(option);

        return InnerCreateApiClient(httpMessageHandler, option);
    }

    private static DataverseApiClient InnerCreateApiClient(
        HttpMessageHandler httpMessageHandler, DataverseApiClientAuthOption option)
    {
        var authenticationHandler = new AuthenticationHandler(httpMessageHandler, option);
        var dataverseHttpApi = new DataverseHttpApi(authenticationHandler, new(option.ServiceUrl, UriKind.Absolute), option.HttpTimeOut);

        return new(dataverseHttpApi, GuidProvider.Instance);
    }
}