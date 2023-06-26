using PrimeFuncPack;
using System;
using System.Net.Http;

namespace GarageGroup.Infra;

public static class DataverseImpersonationDependency
{
    public static Dependency<HttpMessageHandler> UseDataverseImpersonation(
        this Dependency<HttpMessageHandler> dependency, 
        Func<IServiceProvider, IAsyncValueFunc<Guid>> callerIdProviderResolver)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(callerIdProviderResolver);

        return dependency.With(callerIdProviderResolver).Fold<HttpMessageHandler>(ImpersonationDelegatingHandler.Create);
    }

    public static Dependency<HttpMessageHandler> UseDataverseImpersonation(
        this Dependency<HttpMessageHandler, IAsyncValueFunc<Guid>> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Fold<HttpMessageHandler>(ImpersonationDelegatingHandler.Create);
    }
}
