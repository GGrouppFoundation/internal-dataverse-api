using System;
using System.Net.Http;
using Azure.Core;
using Azure.Identity;

namespace GarageGroup.Infra;

internal sealed partial class StandardAzureCredentialHandler : DelegatingHandler
{
    static StandardAzureCredentialHandler()
    {
        LazyCredential = new(CreateCredential);

        static TokenCredential CreateCredential()
            =>
            new ChainedTokenCredential(
                new AzureCliCredential(),
                new ManagedIdentityCredential(),
                new DefaultAzureCredential());
    }

    private static readonly Lazy<TokenCredential> LazyCredential;

    private const string ScopeRelativeUri = "/.default";

    private const string AuthorizationScheme = "Bearer";

    internal StandardAzureCredentialHandler(HttpMessageHandler innerHandler) : base(innerHandler)
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