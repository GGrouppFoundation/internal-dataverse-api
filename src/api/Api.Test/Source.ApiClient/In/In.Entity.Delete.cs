using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> EntityDeleteInputTestData
        =>
        new[]
        {
            new object?[]
            {
                Guid.Parse("91526fc6-1491-4ee9-8b7a-a4ed536de862"),
                new DataverseEntityDeleteIn(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey")),
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Delete,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)",
                    headers: new(
                        CreateCallerIdHeader("91526fc6-1491-4ee9-8b7a-a4ed536de862")),
                    content: default)
            },
            new object?[]
            {
                null,
                new DataverseEntityDeleteIn(
                    entityPluralName: "Some/Entities",
                    entityKey: new StubEntityKey("Some=Key")),
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Delete,
                    url: "/api/data/v9.2/Some%2fEntities(Some=Key)",
                    headers: default,
                    content: default)
            }
        };
}