using GGroupp.Infra;
using PrimeFuncPack;

namespace ConsoleDebug;

public static class Extensions
{
    internal static IDataverseApiClient Create(IServiceProvider sp)
        =>
            PrimaryHandler.UseStandardSocketsHttpHandler()
                .UseLogging("DataverseApiClient")
                .WithOptions()
                .UseDataverseApiClient()
                .Resolve(sp);

    private static Dependency<HttpMessageHandler, DataverseApiClientAuthOption> WithOptions(
        this Dependency<HttpMessageHandler> dependency)
        =>
        dependency
            .With(
                new DataverseApiClientAuthOption(
                    serviceUrl:"https://ppcrm1.crm4.dynamics.com/",
                    authTenantId: Guid.Parse("3a6211b7-143c-4c27-93e9-62671ce276ce"), 
                    authClientId: "9d95d87c-e650-4ecb-8c42-81cc22953368",
                    authClientSecret: "g1b8Q~hVSM4_L5zIBd~tKNjBs6GNvajgBccTHdjR"));
}