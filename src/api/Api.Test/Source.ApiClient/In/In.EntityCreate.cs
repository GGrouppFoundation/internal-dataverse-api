using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> EntityCreateInputTestData
        =>
        new[]
        {
            new object?[]
            {
                null,
                new DataverseEntityCreateIn<StubRequestJson>(
                    entityPluralName: "SomeEntities",
                    entityData:  new()
                    {
                        Id = 17,
                        Name = "First request name"
                    }),
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/data/v9.2/SomeEntities",
                    headers: default,
                    content: new StubRequestJson
                    {
                        Id = 17,
                        Name = "First request name"
                    })
            },
            new object?[]
            {
                Guid.Parse("cf6678d2-2963-4f14-8dff-21c956ae9695"),
                new DataverseEntityCreateIn<StubRequestJson>(
                    entityPluralName: "SomeEntities",
                    selectFields: new[] { string.Empty, "field 1" },
                    entityData: new())
                {
                    ExpandFields = new DataverseExpandedField[]
                    {
                        new("LookupOne", new("field1.1", "field1.2")),
                        new(
                            fieldName: "LookupTwo",
                            selectFields: default,
                            expandFields: new DataverseExpandedField[]
                            {
                                new("field2.1", default)
                            })
                    },
                    SuppressDuplicateDetection = false
                },
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/data/v9.2/SomeEntities",
                    headers: new(
                        CreateCallerIdHeader("cf6678d2-2963-4f14-8dff-21c956ae9695"),
                        CreateSuppressDuplicateDetectionHeader("false")),
                    content: new StubRequestJson())
            },
            new object?[]
            {
                null,
                new DataverseEntityCreateIn<StubRequestJson>(
                    entityPluralName: "Some/Entities",
                    selectFields: default,
                    entityData: new())
                {
                    SuppressDuplicateDetection = true
                },
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/data/v9.2/Some%2fEntities",
                    headers: new(
                        CreateSuppressDuplicateDetectionHeader("true")),
                    content: new StubRequestJson())
            }
        };
}