using System;
using System.Net.Http;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class DataverseApiClientDependency
{
    public static Dependency<IDataverseApiClient> UseDataverseApiClient<TMessageHandler>(
        this Dependency<TMessageHandler, DataverseApiClientConfiguration> dependency)
        where TMessageHandler : HttpMessageHandler
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        return dependency.Fold(InnerCreateApiClient);
    }

    public static Dependency<IDataverseApiClient> UseDataverseApiClient<TMessageHandler>(
        this Dependency<TMessageHandler> dependency,
        Func<IServiceProvider, DataverseApiClientConfiguration> configurationResolver)
        where TMessageHandler : HttpMessageHandler
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = configurationResolver ?? throw new ArgumentNullException(nameof(configurationResolver));

        return dependency.With(configurationResolver).Fold(InnerCreateApiClient);
    }

    private static IDataverseApiClient InnerCreateApiClient<TMessageHandler>(
        TMessageHandler httpMessageHandler,
        DataverseApiClientConfiguration configuration)
        where TMessageHandler : HttpMessageHandler
        =>
        DataverseApiClient.Create(httpMessageHandler, configuration);
}