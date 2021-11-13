using System;
using System.Net.Http;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class DataverseApiClientDependency
{
    public static Dependency<IDataverseApiClient> UseDataverseApiClient<TMessageHandler>(
        this Dependency<TMessageHandler, IFunc<DataverseApiClientConfiguration>> dependency)
        where TMessageHandler : HttpMessageHandler
        => 
        dependency.Fold<IDataverseApiClient>((h, c) => DataverseApiClient.Create(h, c));
}