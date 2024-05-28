using System;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<Guid?, DataverseEntityCreateIn<StubRequestJson>, DataverseJsonRequest> EntityCreateInputTestData
        =>
        new()
        {
            {
                null,
                new(
                    entityPluralName: "SomeEntities",
                    entityData:  new()
                    {
                        Id = 17,
                        Name = "First request name"
                    }),
                new(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/data/v9.2/SomeEntities",
                    headers: default,
                    content: new StubRequestJson
                    {
                        Id = 17,
                        Name = "First request name"
                    }.InnerToJsonContentIn())
            },
            {
                new("cf6678d2-2963-4f14-8dff-21c956ae9695"),
                new(
                    entityPluralName: "SomeEntities",
                    selectFields: new[] { string.Empty, "field 1" },
                    entityData: new())
                {
                    ExpandFields =
                    [
                        new("LookupOne", new("field1.1", "field1.2")),
                        new(
                            fieldName: "LookupTwo",
                            selectFields: default,
                            expandFields:
                            [
                                new("field2.1", default)
                            ])
                    ],
                    SuppressDuplicateDetection = false
                },
                new(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/data/v9.2/SomeEntities",
                    headers:
                    [
                        CreateCallerIdHeader("cf6678d2-2963-4f14-8dff-21c956ae9695"),
                        CreateSuppressDuplicateDetectionHeader("false")
                    ],
                    content: new StubRequestJson().InnerToJsonContentIn())
            },
            {
                null,
                new(
                    entityPluralName: "Some/Entities",
                    selectFields: default,
                    entityData: new())
                {
                    SuppressDuplicateDetection = true
                },
                new(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/data/v9.2/Some%2fEntities",
                    headers:
                    [
                        CreateSuppressDuplicateDetectionHeader("true")
                    ],
                    content: new StubRequestJson().InnerToJsonContentIn())
            }
        };
}