using System;
using System.Net.Http;

namespace GGroupp.Infra;

internal sealed partial class ImpersonationDelegatingHandler : DelegatingHandler
{
    private const string CallerIdHeaderName = "MSCRMCallerID";

    public static ImpersonationDelegatingHandler Create(HttpMessageHandler innerHandler, IAsyncValueFunc<Guid> callerIdProvider)
        => 
        new(
            innerHandler ?? throw new ArgumentNullException(nameof(innerHandler)),
            callerIdProvider ?? throw new ArgumentNullException(nameof(callerIdProvider)));

    private readonly IAsyncValueFunc<Guid> callerIdProvider;

    private ImpersonationDelegatingHandler(HttpMessageHandler innerHandler, IAsyncValueFunc<Guid> callerIdProvider) : base(innerHandler)
        =>
        this.callerIdProvider = callerIdProvider;
}