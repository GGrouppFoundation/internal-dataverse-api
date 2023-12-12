using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<DataverseEntitySetJsonGetOut<StubResponseJson>, DataverseEntitySetGetOut<StubResponseJson>> StubResponseSetOutputTestData
        =>
        new()
        {
            {
                new(),
                new(default)
            },
            {
                new()
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
                new(
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
            {
                new()
                {
                    NextLink = "Some Link"
                },
                new(
                    value: default,
                    nextLink: "Some Link")
            },
            {
                new()
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
                new(
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