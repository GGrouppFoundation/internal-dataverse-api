using System.Collections.Generic;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> StubResponseJsonOutputTestData
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