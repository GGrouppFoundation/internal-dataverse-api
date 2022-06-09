using PrimeFuncPack;
using System;
using System.Net.Http;

namespace GGroupp.Infra;

public static class DataverseImpersonationDependency
{
    public static Dependency<HttpMessageHandler> UseDataverseImpersonation(
        this Dependency<HttpMessageHandler> dependency, 
        Func<IServiceProvider, IAsyncValueFunc<Guid>> callerIdProviderResolver)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = callerIdProviderResolver ?? throw new ArgumentNullException(nameof(callerIdProviderResolver));

        return dependency.With(callerIdProviderResolver).Fold<HttpMessageHandler>(ImpersonationDelegatingHandler.Create);
    }

    public static Dependency<HttpMessageHandler> UseDataverseImpersonation(
        this Dependency<HttpMessageHandler, IAsyncValueFunc<Guid>> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold<HttpMessageHandler>(ImpersonationDelegatingHandler.Create);
    }
}
