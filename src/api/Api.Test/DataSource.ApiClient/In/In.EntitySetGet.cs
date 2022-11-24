using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetEntitySetGetInputTestData()
        =>
        new[]
        {
            new object?[]
            {
                null,
                new DataverseEntitySetGetIn(
                    entityPluralName: "SomeEntities",
                    selectFields: new("field1", "field2"),
                    filter: "id eq 15",
                    orderBy: new DataverseOrderParameter[]
                    {
                        new("field3", DataverseOrderDirection.Descending),
                        new("field4"),
                        new("field5", DataverseOrderDirection.Ascending)
                    },
                    top: 15),
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/SomeEntities?$select=field1,field2&$filter=id eq 15&$orderby=field3 desc,field4,field5 asc&$top=15",
                    headers: default,
                    content: default)
            },
            new object?[]
            {
                null,
                new DataverseEntitySetGetIn(
                    entityPluralName: "Some Entities",
                    selectFields: new(string.Empty, "field 1"),
                    filter: default,
                    orderBy: default,
                    top: 1)
                    {
                        IncludeAnnotations = "display.*"
                    },
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/Some+Entities?$select=field 1&$top=1",
                    headers: new(
                        new DataverseHttpHeader("Prefer", "odata.include-annotations=display.*")),
                    content: default)
            },
            new object?[]
            {
                null,
                new DataverseEntitySetGetIn(
                    entityPluralName: "Some/Entities",
                    selectFields: default,
                    filter: "date gt 2020-01-01")
                    {
                        IncludeAnnotations = "*"
                    },
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/Some%2fEntities?$filter=date gt 2020-01-01",
                    headers: new(
                        new DataverseHttpHeader("Prefer", "odata.include-annotations=*")),
                    content: default)
            },
            new object?[]
            {
                null,
                new DataverseEntitySetGetIn(
                    entityPluralName: "SomeEntities",
                    selectFields: new("FieldOne"),
                    filter: string.Empty)
                    {
                        MaxPageSize = 10
                    },
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/SomeEntities?$select=FieldOne",
                    headers: new(
                        new DataverseHttpHeader("Prefer", "odata.maxpagesize=10")),
                    content: default)
            },
            new object?[]
            {
                Guid.Parse("d44c6578-1f2e-4edd-8897-77aaf8bd524a"),
                new DataverseEntitySetGetIn(
                    entityPluralName: "SomeEntities",
                    selectFields: default,
                    filter: default)
                    {
                        MaxPageSize = -5,
                        IncludeAnnotations = "*"
                    },
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/SomeEntities",
                    headers: new(
                        CreateCallerIdHeader("d44c6578-1f2e-4edd-8897-77aaf8bd524a"),
                        new("Prefer", "odata.maxpagesize=-5,odata.include-annotations=*")),
                    content: default)
            },
            new object?[]
            {
                null,
                new DataverseEntitySetGetIn("http://garage.ru/api/someLink")
                {
                    MaxPageSize = 15,
                    IncludeAnnotations = "*"
                },
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "http://garage.ru/api/someLink",
                    headers: new(
                        new DataverseHttpHeader("Prefer", "odata.maxpagesize=15,odata.include-annotations=*")),
                    content: default)
            },
            new object?[]
            {
                null,
                new DataverseEntitySetGetIn("https://garage.ru/api/someLink")
                {
                    MaxPageSize = 7
                },
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "https://garage.ru/api/someLink",
                    headers: new(
                        new DataverseHttpHeader("Prefer", "odata.maxpagesize=7")),
                    content: default)
            },
            new object?[]
            {
                null,
                new DataverseEntitySetGetIn("/api/someLink")
                {
                    IncludeAnnotations = "*"
                },
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/someLink",
                    headers: new(
                        new DataverseHttpHeader("Prefer", "odata.include-annotations=*")),
                    content: default)
            },
            new object?[]
            {
                Guid.Parse("be070c0c-3cf5-44a4-8eb2-b9b4a686024b"),
                new DataverseEntitySetGetIn("http://garage.ru/api/someLink"),
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "http://garage.ru/api/someLink",
                    headers: new(
                        CreateCallerIdHeader("be070c0c-3cf5-44a4-8eb2-b9b4a686024b")),
                    content: default)
            }
        };
}