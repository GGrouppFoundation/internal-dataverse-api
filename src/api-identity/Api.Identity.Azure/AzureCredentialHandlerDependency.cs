using System;
using System.Collections.Concurrent;
using System.Net.Http;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class AzureCredentialHandlerDependency
{
    private const string StandardClientIdKey = "AZURE_CLIENT_ID";

    private static readonly Lazy<AzureCliCredential> CliCredential;

    private static readonly ConcurrentDictionary<string, ManagedIdentityCredential> ManagedIdentityCredentials;

    static AzureCredentialHandlerDependency()
    {
        CliCredential = new(CreateCliCredential);
        ManagedIdentityCredentials = new();
    }

    public static Dependency<HttpMessageHandler> UseDataverseAzureCredentialStandard(
        this Dependency<HttpMessageHandler> dependency, string clientIdKey = StandardClientIdKey)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<HttpMessageHandler>(CreateHandler);

        AzureCredentialHandler CreateHandler(IServiceProvider serviceProvider, HttpMessageHandler innerHandler)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(innerHandler);

            return new(innerHandler, serviceProvider.ResolveTokenCredential(clientIdKey ?? string.Empty));
        }
    }

    private static TokenCredential ResolveTokenCredential(this IServiceProvider serviceProvider, string clientIdKey)
    {
        var clientId = serviceProvider.GetServiceOrAbsent<IConfiguration>().OrDefault()?[clientIdKey];

        if (string.IsNullOrEmpty(clientId))
        {
            return CliCredential.Value;
        }

        return ManagedIdentityCredentials.GetOrAdd(clientId, CreateManagedIdentityCredential);
    }

    private static AzureCliCredential CreateCliCredential()
        =>
        new();

    private static ManagedIdentityCredential CreateManagedIdentityCredential(string clientId)
        =>
        new(clientId: clientId);
}