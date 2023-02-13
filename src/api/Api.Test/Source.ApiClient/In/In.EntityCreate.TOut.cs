using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> EntityCreateWithTOutInputTestData
        =>
        new[]
        {
            new object?[]
            {
                null,
                new DataverseEntityCreateIn<StubRequestJson>(
                    entityPluralName: "SomeEntities",
                    selectFields: new[] { "field1", "field2" },
                    entityData:  new()
                    {
                        Id = 17,
                        Name = "First request name"
                    }),
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/data/v9.2/SomeEntities?$select=field1,field2",
                    headers: new(PreferRepresentationHeader),
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
                    url: "/api/data/v9.2/SomeEntities?$select=field 1" +
                        "&$expand=LookupOne($select=field1.1,field1.2),LookupTwo($expand=field2.1)",
                    headers: new(
                        CreateCallerIdHeader("cf6678d2-2963-4f14-8dff-21c956ae9695"),
                        PreferRepresentationHeader,
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
                        PreferRepresentationHeader,
                        CreateSuppressDuplicateDetectionHeader("true")),
                    content: new StubRequestJson())
            }
        };
}