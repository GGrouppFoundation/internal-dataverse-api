using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;
using static Microsoft.Extensions.Configuration.DataverseApiClientConfigurationExtensions;

[assembly: InternalsVisibleTo("GGroupp.Infra.Dataverse.Api.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GGroupp.Infra;

public static class DataverseApiClientDependency
{
    public static Dependency<IDataverseApiClient> UseDataverseApiClient(
        this Dependency<HttpMessageHandler, DataverseApiClientAuthOption> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold(CreateApiClient);
    }

    public static Dependency<IDataverseApiClient> UseDataverseApiClient(
        this Dependency<HttpMessageHandler> dependency, Func<IServiceProvider, DataverseApiClientAuthOption> optionResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).Fold(CreateApiClient);
    }

    public static Dependency<IDataverseApiClient> UseDataverseApiClient(
        this Dependency<HttpMessageHandler> dependency, [AllowNull] string sectionName = DefaultSectionName)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.With(ResolveOption).Fold(CreateApiClient);

        DataverseApiClientAuthOption ResolveOption(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetServiceOrThrow<IConfiguration>().InternalGetDataverseApiClientAuthOption(sectionName.OrEmpty());
    }

    private static IDataverseApiClient CreateApiClient(
        HttpMessageHandler httpMessageHandler, DataverseApiClientAuthOption option)
    {
        _ = httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler));
        _ = option ?? throw new ArgumentNullException(nameof(option));

        var authenticationHandler = new AuthenticationHandler(httpMessageHandler, option);
        var dataverseHttpApi = new DataverseHttpApi(authenticationHandler, new(option.ServiceUrl, UriKind.Absolute));

        return new DataverseApiClient(dataverseHttpApi);
    }
}