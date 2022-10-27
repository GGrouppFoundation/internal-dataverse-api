using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetStubResponseJsonSetOutputTestData()
    {
        yield return new object?[]
        {
            null,
            FlatArray.Empty<StubResponseJson>()
        };

        yield return new object?[]
        {
            new StringContent(string.Empty),
            FlatArray.Empty<StubResponseJson>()
        };

        var emptyResponseJson = new DataverseEntitySetJsonGetOut<StubResponseJson>();

        yield return new object?[]
        {
            CreateResponseContentJson(emptyResponseJson),
            FlatArray.Empty<StubResponseJson>()
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
            responseJson.Value.ToFlatArray()
        };
    }
}