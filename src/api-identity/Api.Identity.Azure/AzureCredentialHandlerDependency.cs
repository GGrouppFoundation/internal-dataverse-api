using System;
using System.Net.Http;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class AzureCredentialHandlerDependency
{
    public static Dependency<HttpMessageHandler> UseDataverseAzureCredentialStandard(
        this Dependency<HttpMessageHandler> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<HttpMessageHandler>(CreateHandler);

        static StandardAzureCredentialHandler CreateHandler(HttpMessageHandler innerHandler)
        {
            ArgumentNullException.ThrowIfNull(innerHandler);
            return new(innerHandler);
        }
    }
}