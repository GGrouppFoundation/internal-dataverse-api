using System;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<Guid?, DataverseEntityDeleteIn, DataverseJsonRequest> EntityDeleteInputTestData
        =>
        new()
        {
            {
                Guid.Parse("91526fc6-1491-4ee9-8b7a-a4ed536de862"),
                new(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey")),
                new(
                    verb: DataverseHttpVerb.Delete,
                    url: "/api/data/v9.2/SomeEntities(SomeKey)",
                    headers: new(
                        CreateCallerIdHeader("91526fc6-1491-4ee9-8b7a-a4ed536de862")),
                    content: default)
            },
            {
                null,
                new(
                    entityPluralName: "Some/Entities",
                    entityKey: new StubEntityKey("Some=Key")),
                new(
                    verb: DataverseHttpVerb.Delete,
                    url: "/api/data/v9.2/Some%2fEntities(Some=Key)",
                    headers: default,
                    content: default)
            }
        };
}