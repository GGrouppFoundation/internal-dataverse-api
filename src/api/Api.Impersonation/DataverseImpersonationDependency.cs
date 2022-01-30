using PrimeFuncPack;
using System;
using System.Net.Http;

namespace GGroupp.Infra;

public static class DataverseImpersonationDependency
{
    public static Dependency<DelegatingHandler> UseDataverseImpersonation<THandler>(
        this Dependency<THandler> dependency, 
        Func<IServiceProvider, IAsyncValueFunc<Guid>> callerIdProviderResolver)
        where THandler : HttpMessageHandler
        =>
        InnerUseDataverseImpersonation(
            dependency ?? throw new ArgumentNullException(nameof(dependency)),
            callerIdProviderResolver ?? throw new ArgumentNullException(nameof(callerIdProviderResolver)));

    private static Dependency<DelegatingHandler> InnerUseDataverseImpersonation<THandler>(
        Dependency<THandler> dependency, 
        Func<IServiceProvider, IAsyncValueFunc<Guid>> callerIdProviderResolver)
        where THandler : HttpMessageHandler
        =>
        dependency.With(callerIdProviderResolver).Fold<DelegatingHandler>(ImpersonationDelegatingHandler.Create);
}
