using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> EntityUpdateInputTestData
        =>
        new[]
        {
            new object?[]
            {
                Guid.Parse("9fdea890-f164-47c1-bb51-d3865229fa9b"),
                new DataverseEntityUpdateIn<StubRequestJson>(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey"),
                    entityData: new()
                    {
                        Id = 101,
                        Name = "First request name"
                    })
                {
                    SuppressDuplicateDetection = true
                },
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Patch,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)",
                    headers: new(
                        CreateCallerIdHeader("9fdea890-f164-47c1-bb51-d3865229fa9b"),
                        CreateSuppressDuplicateDetectionHeader("true")),
                    content: new StubRequestJson
                    {
                        Id = 101,
                        Name = "First request name"
                    })
            },
            new object?[]
            {
                null,
                new DataverseEntityUpdateIn<StubRequestJson>(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey"),
                    selectFields: new[] { string.Empty, "field 1" },
                    entityData: new())
                {
                    SuppressDuplicateDetection = false
                },
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Patch,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)",
                    headers: new(
                        CreateSuppressDuplicateDetectionHeader("false")),
                    content: new StubRequestJson())
            },
            new object?[]
            {
                null,
                new DataverseEntityUpdateIn<StubRequestJson>(
                    entityPluralName: "Some/Entities",
                    entityKey: new StubEntityKey("Some Key"),
                    selectFields: default,
                    entityData: new())
                {
                    ExpandFields = new DataverseExpandedField[]
                    {
                        new("Field1", new("Lookup Field"))
                    }
                },
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Patch,
                    url: "/api/data/v9.2/Some%2fEntities(Some Key)",
                    headers: default,
                    content: new StubRequestJson())
            }
        };
}