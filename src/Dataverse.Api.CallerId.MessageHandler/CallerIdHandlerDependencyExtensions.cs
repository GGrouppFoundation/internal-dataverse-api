using PrimeFuncPack;
using System;
using System.Net.Http;

namespace GGroupp.Infra;

public static class CallerIdHandlerDependencyExtensions
{
    public static Dependency<DelegatingHandler> UseCallerId<THandler>(
        this Dependency<THandler> dependency, 
        Func<IServiceProvider, IAsyncValueFunc<Guid>> callerIdProviderResolver)
        where THandler : HttpMessageHandler
        =>
        InnerUseCallerId(
            dependency ?? throw new ArgumentNullException(nameof(dependency)),
            callerIdProviderResolver ?? throw new ArgumentNullException(nameof(callerIdProviderResolver)));

    private static Dependency<DelegatingHandler> InnerUseCallerId<THandler>(
        Dependency<THandler> dependency, 
        Func<IServiceProvider, IAsyncValueFunc<Guid>> callerIdProviderResolver)
        where THandler : HttpMessageHandler
        =>
        dependency.With(callerIdProviderResolver).Fold<DelegatingHandler>(CallerIdDelegatingHandler.Create);
}
