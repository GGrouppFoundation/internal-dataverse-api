using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetEntityDeleteInputTestData()
        =>
        new[]
        {
            new object?[]
            {
                new Uri("https://some.crm4.dynamics.com/", UriKind.Absolute),
                new DataverseEntityDeleteIn(
                    entityPluralName: "SomeEntities",
                    entityKey: new StubEntityKey("SomeKey")),
                "https://some.crm4.dynamics.com/api/data/v9.0/SomeEntities(SomeKey)"
            },
            new object?[]
            {
                new Uri("http://ggroupp.ru", UriKind.Absolute),
                new DataverseEntityDeleteIn(
                    entityPluralName: "Some/Entities",
                    entityKey: new StubEntityKey("Some=Key")),
                "http://ggroupp.ru/api/data/v9.0/Some%2fEntities(Some=Key)"
            }
        };
}