using System;
using System.Collections.Generic;
using System.Net.Http;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetStubResponseJsonSetTestDataPair()
    {
        yield return new object?[]
        {
            null,
            Array.Empty<StubResponseJson>()
        };

        yield return new object?[]
        {
            new StringContent(string.Empty),
            Array.Empty<StubResponseJson>()
        };

        var emptyResponseJson = new DataverseEntitySetJsonGetOut<StubResponseJson>();

        yield return new object?[]
        {
            CreateResponseContentJson(emptyResponseJson),
            Array.Empty<StubResponseJson>()
        };

        var responseJson = new DataverseEntitySetJsonGetOut<StubResponseJson>
        {
            Value = new StubResponseJson[]
            {
                new()
                {
                    Id = 15,
                    Name = "Some first"
                },
                new()
                {
                    Id = 101,
                    Name = "Some second"
                }
            }
        };

        yield return new object?[]
        {
            CreateResponseContentJson(responseJson),
            responseJson.Value
        };
    }
}