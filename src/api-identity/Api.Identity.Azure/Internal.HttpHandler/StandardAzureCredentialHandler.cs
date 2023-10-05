using System;
using System.Net.Http;
using Azure.Core;

namespace GarageGroup.Infra;

internal sealed partial class AzureCredentialHandler : DelegatingHandler
{
    private const string ScopeRelativeUri = "/.default";

    private const string AuthorizationScheme = "Bearer";

    private readonly TokenCredential tokenCredential;

    internal AzureCredentialHandler(HttpMessageHandler innerHandler, TokenCredential tokenCredential) : base(innerHandler)
        =>
        this.tokenCredential = tokenCredential;

    private static TokenRequestContext CreateRequestContext(Uri requestUri)
        =>
#if NET8_0_OR_GREATER
        new(
            scopes:
            [
                new Uri(requestUri, ScopeRelativeUri).ToString()
            ]);
#else
        new(
            scopes: new[]
            {
                new Uri(requestUri, ScopeRelativeUri).ToString()
            });
#endif
}