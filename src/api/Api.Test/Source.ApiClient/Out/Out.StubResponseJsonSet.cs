using System.Collections.Generic;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> StubResponseJsonSetOutputTestData
        =>
        new[]
        {
            new object?[]
            {
                new DataverseEntitySetJsonGetOut<StubResponseJson>(),
                new DataverseEntitySetGetOut<StubResponseJson>(default)
            },
            new object?[]
            {
                new DataverseEntitySetJsonGetOut<StubResponseJson>
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
                },
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
            },
            new object?[]
            {
                new DataverseEntitySetJsonGetOut<StubResponseJson>
                {
                    NextLink = "Some Link"
                },
                new DataverseEntitySetGetOut<StubResponseJson>(
                    value: default,
                    nextLink: "Some Link")
            },
            new object?[]
            {
                new DataverseEntitySetJsonGetOut<StubResponseJson>
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
                },
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
            }
        };
}