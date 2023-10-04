using System;
using System.Net.Http;
using Azure.Core;
using Azure.Identity;

namespace GarageGroup.Infra;

internal sealed partial class DefaultAzureCredentialHandler : DelegatingHandler
{
    static DefaultAzureCredentialHandler()
    {
        LazyCredential = new(CreateCredential);

        static TokenCredential CreateCredential()
            =>
            new DefaultAzureCredential();
    }

    private static readonly Lazy<TokenCredential> LazyCredential;

    private const string ScopeRelativeUri = "/.default";

    private const string AuthorizationScheme = "Bearer";

    internal DefaultAzureCredentialHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {
    }

    private static TokenRequestContext CreateRequestContext(Uri requestUri)
        =>
        new(
            scopes: new[]
            {
                new Uri(requestUri, ScopeRelativeUri).ToString()
            });
}