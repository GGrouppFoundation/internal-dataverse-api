using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> EntityUpdateWithTOutInputTestData
        =>
        new[]
        {
            new object?[]
            {
                Guid.Parse("9fdea890-f164-47c1-bb51-d3865229fa9b"),
                new DataverseEntityUpdateIn<StubRequestJson>(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey"),
                    selectFields: new[] { "field1", "field2" },
                    entityData: new()
                    {
                        Id = 101,
                        Name = "First request name"
                    })
                {
                    SuppressDuplicateDetection = true
                },
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Patch,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)?$select=field1,field2",
                    headers: new(
                        CreateCallerIdHeader("9fdea890-f164-47c1-bb51-d3865229fa9b"),
                        PreferRepresentationHeader,
                        CreateSuppressDuplicateDetectionHeader("true")),
                    content: new StubRequestJson
                    {
                        Id = 101,
                        Name = "First request name"
                    }.InnerToJsonContentIn())
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
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Patch,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)?$select=field 1",
                    headers: new(
                        PreferRepresentationHeader,
                        CreateSuppressDuplicateDetectionHeader("false")),
                    content: new StubRequestJson().InnerToJsonContentIn())
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
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Patch,
                    url: "/api/data/v9.2/Some%2fEntities(Some Key)?$expand=Field1($select=Lookup Field)",
                    headers: new(PreferRepresentationHeader),
                    content: new StubRequestJson().InnerToJsonContentIn())
            }
        };
}