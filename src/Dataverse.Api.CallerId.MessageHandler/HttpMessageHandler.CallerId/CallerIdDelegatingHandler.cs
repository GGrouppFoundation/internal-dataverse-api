using System;
using System.Net.Http;

namespace GGroupp.Infra;

internal sealed partial class CallerIdDelegatingHandler : DelegatingHandler
{
    public static CallerIdDelegatingHandler Create(HttpMessageHandler innerHandler, IAsyncValueFunc<Guid> callerIdProvider)
        => 
        new(
            innerHandler ?? throw new ArgumentNullException(nameof(innerHandler)),
            callerIdProvider ?? throw new ArgumentNullException(nameof(callerIdProvider)));

    private readonly IAsyncValueFunc<Guid> callerIdProvider;

    private const string MSCRMCallerID = "MSCRMCallerID";

    private CallerIdDelegatingHandler(HttpMessageHandler innerHandler, IAsyncValueFunc<Guid> callerIdProvider) : base(innerHandler)
        =>
        this.callerIdProvider = callerIdProvider;
}