#nullable enable

using System.Net.Http;
using PrimeFuncPack;

namespace GGroupp
{
    public static class DataVerseApiClientDependencyExtensions
    {
        public static Dependency<IDataverseApiClient> UseDataverseApiClient<TMessageHandler, TConfiguration>(
            this Dependency<TMessageHandler, TConfiguration> dependency)
            where TMessageHandler : HttpMessageHandler
            where TConfiguration : IDataverseApiClientConfiguration
            => 
            dependency.Fold<IDataverseApiClient>((h, c) => DataverseApiClient.Create(h, c));
    }
}