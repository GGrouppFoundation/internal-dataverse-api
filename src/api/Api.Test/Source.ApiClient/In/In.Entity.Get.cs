using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> EntityGetInputTestData
        =>
        new[]
        {
            new object?[]
            {
                Guid.Parse("18945ff7-9433-4e74-a403-abd6db25ef27"),
                new DataverseEntityGetIn(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey"),
                    selectFields: new("field1", "field2"),
                    expandFields: new(
                        new("LookupOne", new("field1.1", "field1.2", string.Empty)),
                        new(string.Empty, new("field 2.1", "field 2.2")),
                        new(
                            fieldName: "LookupThree",
                            selectFields: new("field3.1")))),
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)?$select=field1,field2&" +
                        "$expand=LookupOne($select=field1.1,field1.2),LookupThree($select=field3.1)",
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
                    selectFields: new(string.Empty, "field 1"))
                    {
                        IncludeAnnotations = "display.*"
                    },
                new DataverseJsonRequest(
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
                    selectFields: default,
                    expandFields: new(
                        new DataverseExpandedField(
                            fieldName: "SomeField",
                            selectFields: default,
                            expandFields: new(
                                new DataverseExpandedField(
                                    fieldName: "Lookup1",
                                    selectFields: new("Field1", "Field2"),
                                    expandFields: new(
                                        new DataverseExpandedField("Lookup3", default)))))))
                    {
                        IncludeAnnotations = "*"
                    },
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/Some%2fEntities(Some=Key)?$expand=SomeField($expand=Lookup1($select=Field1,Field2;$expand=Lookup3))",
                    headers: new(
                        CreateCallerIdHeader("18945ff7-9433-4e74-a403-abd6db25ef27"),
                        new DataverseHttpHeader("Prefer", "odata.include-annotations=*")),
                    content: default)
            },
            new object?[]
            {
                Guid.Parse("18945ff7-9433-4e74-a403-abd6db25ef27"),
                new DataverseEntityGetIn(
                    entityPluralName: "Some/Entities",
                    entityKey: new StubEntityKey("Some=Key"),
                    selectFields: default,
                    expandFields: default)
                    {
                        IncludeAnnotations = "*"
                    },
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/Some%2fEntities(Some=Key)",
                    headers: new(
                        CreateCallerIdHeader("18945ff7-9433-4e74-a403-abd6db25ef27"),
                        new DataverseHttpHeader("Prefer", "odata.include-annotations=*")),
                    content: default)
            },
            new object?[]
            {
                Guid.Parse("18945ff7-9433-4e74-a403-abd6db25ef27"),
                new DataverseEntityGetIn(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey"),
                    selectFields: new("field1", "field2"),
                    expandFields: new DataverseExpandedField("LookupOne").AsFlatArray()),
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)?$select=field1,field2&$expand=LookupOne",
                    headers: new(
                        CreateCallerIdHeader("18945ff7-9433-4e74-a403-abd6db25ef27")),
                    content: default)
            },
        };
}