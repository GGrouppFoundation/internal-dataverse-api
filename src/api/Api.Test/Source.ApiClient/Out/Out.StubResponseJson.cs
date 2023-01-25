using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetStubResponseJsonOutputTestData()
        =>
        new[]
        {
            new object?[]
            {
                default(StubResponseJson)
            },
            new object?[]
            {
                new StubResponseJson
                {
                    Id = 15,
                    Name = "Some name"
                }
            }
        };
}