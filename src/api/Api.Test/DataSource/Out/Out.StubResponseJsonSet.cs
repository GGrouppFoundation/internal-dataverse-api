using System.Collections.Generic;
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

        yield return new object?[]
        {
            new StubEntitySetJsonOut().CreateResponseContentJson(),
            new DataverseEntitySetGetOut<StubResponseJson>(default)
        };

        yield return new object?[]
        {
            new StubEntitySetJsonOut
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
            }
            .CreateResponseContentJson(),
            new DataverseEntitySetGetOut<StubResponseJson>(
                value: new(
                    new StubResponseJson
                    {
                        Id = 15,
                        Name = "Some first"
                    },
                    new StubResponseJson
                    {
                        Id = 101,
                        Name = "Some second"
                    }))
        };

        yield return new object?[]
        {
            new StubEntitySetJsonOut
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
            }
            .CreateResponseContentJson(),
            new DataverseEntitySetGetOut<StubResponseJson>(
                value: new(
                    new StubResponseJson
                    {
                        Id = -11,
                        Name = "First"
                    }))
        };

        yield return new object?[]
        {
            new StubEntitySetJsonOut
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
            }
            .CreateResponseContentJson(),
            new DataverseEntitySetGetOut<StubResponseJson>(
                value: new(
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
                    }),
                nextLink: "Some Link")
        };
    }
}