using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GGroupp.Infra.Dataverse.Api.Test")]

namespace GGroupp.Infra;

public static class DataverseApiClientDependency
{
    public static Dependency<IDataverseApiClient> UseDataverseApiClient<TMessageHandler>(
        this Dependency<TMessageHandler, DataverseApiClientConfiguration> dependency)
        where TMessageHandler : HttpMessageHandler
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        return dependency.Fold(CreateApiClient);
    }

    public static Dependency<IDataverseApiClient> UseDataverseApiClient<TMessageHandler>(
        this Dependency<TMessageHandler> dependency,
        Func<IServiceProvider, DataverseApiClientConfiguration> configurationResolver)
        where TMessageHandler : HttpMessageHandler
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = configurationResolver ?? throw new ArgumentNullException(nameof(configurationResolver));

        return dependency.With(configurationResolver).Fold(CreateApiClient);
    }

    private static IDataverseApiClient CreateApiClient<TMessageHandler>(
        TMessageHandler httpMessageHandler,
        DataverseApiClientConfiguration configuration)
        where TMessageHandler : HttpMessageHandler
    {
        _ = httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler));
        _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

        var authenticationHandler = new AuthenticationHandler(httpMessageHandler, configuration);
        return new DataverseApiClient(authenticationHandler, new(configuration.ServiceUrl, UriKind.Absolute));
    }
}