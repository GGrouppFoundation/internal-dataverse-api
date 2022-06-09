using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GGroupp.Infra.Dataverse.Api.Test")]

namespace GGroupp.Infra;

public static class DataverseApiClientDependency
{
    public static Dependency<IDataverseApiClient> UseDataverseApiClient(
        this Dependency<HttpMessageHandler, DataverseApiClientOption> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold(CreateApiClient);
    }

    public static Dependency<IDataverseApiClient> UseDataverseApiClient(
        this Dependency<HttpMessageHandler> dependency, Func<IServiceProvider, DataverseApiClientOption> optionResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).Fold(CreateApiClient);
    }

    private static IDataverseApiClient CreateApiClient(HttpMessageHandler httpMessageHandler, DataverseApiClientOption option)
    {
        _ = httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler));
        _ = option ?? throw new ArgumentNullException(nameof(option));

        var authenticationHandler = new AuthenticationHandler(httpMessageHandler, option);
        return new DataverseApiClient(authenticationHandler, new(option.ServiceUrl, UriKind.Absolute));
    }
}