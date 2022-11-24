using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetEntityGetInputTestData()
        =>
        new[]
        {
            new object?[]
            {
                Guid.Parse("18945ff7-9433-4e74-a403-abd6db25ef27"),
                new DataverseEntityGetIn(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey"),
                    selectFields: new[] { "field1", "field2" }),
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)?$select=field1,field2",
                    headers: new(
                        CreateCallerIdHeader("18945ff7-9433-4e74-a403-abd6db25ef27")),
                    content: default)
            },
            new object?[]
            {
                null,
                new DataverseEntityGetIn(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey"),
                    selectFields: new[] { string.Empty, "field 1" })
                    {
                        IncludeAnnotations = "display.*"
                    },
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)?$select=field 1",
                    headers: new(
                        new DataverseHttpHeader("Prefer", "odata.include-annotations=display.*")),
                    content: default)
            },
            new object?[]
            {
                Guid.Parse("18945ff7-9433-4e74-a403-abd6db25ef27"),
                new DataverseEntityGetIn(
                    entityPluralName: "Some/Entities",
                    entityKey: new StubEntityKey("Some=Key"),
                    selectFields: default)
                    {
                        IncludeAnnotations = "*"
                    },
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/Some%2fEntities(Some=Key)",
                    headers: new(
                        CreateCallerIdHeader("18945ff7-9433-4e74-a403-abd6db25ef27"),
                        new DataverseHttpHeader("Prefer", "odata.include-annotations=*")),
                    content: default)
            }
        };
}