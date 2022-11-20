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
            new DataverseEntitySetGetOut<StubResponseJson>(default)
        };

        yield return new object?[]
        {
            new StringContent(string.Empty),
            new DataverseEntitySetGetOut<StubResponseJson>(default)
        };

        var emptyResponseJson = new DataverseEntitySetJsonGetOut<StubResponseJson>();

        yield return new object?[]
        {
            CreateResponseContentJson(emptyResponseJson),
            new DataverseEntitySetGetOut<StubResponseJson>(default)
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
            new DataverseEntitySetGetOut<StubResponseJson>(responseJson.Value)
        };

        var responseEmptyNextLinkJson = new DataverseEntitySetJsonGetOut<StubResponseJson>
        {
            Value = new StubResponseJson[]
            {
                new()
                {
                    Id = -11,
                    Name = "First"
                }
            },
            NextLink = string.Empty
        };

        yield return new object?[]
        {
            CreateResponseContentJson(responseEmptyNextLinkJson),
            new DataverseEntitySetGetOut<StubResponseJson>(responseEmptyNextLinkJson.Value)
        };

        var responseNotEmptyNextLinkJson = new DataverseEntitySetJsonGetOut<StubResponseJson>
        {
            Value = new StubResponseJson[]
            {
                new()
                {
                    Id = 171,
                    Name = string.Empty
                },
                new()
                {
                    Id = 0,
                    Name = "Second"
                },
                new()
                {
                    Id = -105,
                    Name = "Third"
                }
            },
            NextLink = "Some Link"
        };

        yield return new object?[]
        {
            CreateResponseContentJson(responseNotEmptyNextLinkJson),
            new DataverseEntitySetGetOut<StubResponseJson>(responseNotEmptyNextLinkJson.Value, responseNotEmptyNextLinkJson.NextLink)
        };
    }
}