using System;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<Guid?, DataverseEntityUpdateIn<StubRequestJson>, DataverseJsonRequest> EntityUpdateInputTestData
        =>
        new()
        {
            {
                Guid.Parse("9fdea890-f164-47c1-bb51-d3865229fa9b"),
                new(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey"),
                    entityData: new()
                    {
                        Id = 101,
                        Name = "First request name"
                    })
                {
                    SuppressDuplicateDetection = true,
                    OperationType = DataverseUpdateOperationType.Update
                },
                new(
                    verb: DataverseHttpVerb.Patch,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)",
                    headers: new[]
                    {
                        CreateCallerIdHeader("9fdea890-f164-47c1-bb51-d3865229fa9b"),
                        CreateSuppressDuplicateDetectionHeader("true"),
                        new("If-Match", "*")
                    },
                    content: new StubRequestJson
                    {
                        Id = 101,
                        Name = "First request name"
                    }.InnerToJsonContentIn())
            },
            {
                null,
                new(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey"),
                    selectFields: new(string.Empty, "field 1"),
                    entityData: new())
                {
                    SuppressDuplicateDetection = false,
                    OperationType = (DataverseUpdateOperationType)2
                },
                new(
                    verb: DataverseHttpVerb.Patch,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)",
                    headers: new[]
                    {
                        CreateSuppressDuplicateDetectionHeader("false"),
                        new("If-Match", "*")
                    },
                    content: new StubRequestJson().InnerToJsonContentIn())
            },
            {
                null,
                new(
                    entityPluralName: "Some/Entities",
                    entityKey: new StubEntityKey("Some Key"),
                    selectFields: default,
                    entityData: new())
                {
                    ExpandFields = new DataverseExpandedField[]
                    {
                        new("Field1", new("Lookup Field"))
                    },
                    OperationType = DataverseUpdateOperationType.Upsert
                },
                new(
                    verb: DataverseHttpVerb.Patch,
                    url: "/api/data/v9.2/Some%2fEntities(Some Key)",
                    headers: default,
                    content: new StubRequestJson().InnerToJsonContentIn())
            }
        };
}