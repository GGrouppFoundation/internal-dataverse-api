using System;
using System.Net.Http;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class DefaultAzureCredentialHandlerDependency
{
    public static Dependency<HttpMessageHandler> UseDataverseDefaultAzureCredential(
        this Dependency<HttpMessageHandler> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<HttpMessageHandler>(CreateHandler);

        static DefaultAzureCredentialHandler CreateHandler(HttpMessageHandler innerHandler)
        {
            ArgumentNullException.ThrowIfNull(innerHandler);
            return new(innerHandler);
        }
    }
}