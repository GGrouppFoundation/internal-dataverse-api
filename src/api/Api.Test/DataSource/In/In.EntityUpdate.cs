using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object[]> GetEntityUpdateInputTestData()
    {
        var firstRequest = new StubRequestJson
        {
            Id = 101,
            Name = "First request name"
        };

        yield return new object[]
        {
            new Uri("https://some.crm4.dynamics.com/", UriKind.Absolute),
            new DataverseEntityUpdateIn<StubRequestJson>(
                entityPluralName: "SomeEntities",
                entityKey: new StubEntityKey("SomeKey"),
                selectFields: new[] { "field1", "field2" },
                entityData: firstRequest),
            "https://some.crm4.dynamics.com/api/data/v9.0/SomeEntities(SomeKey)?$select=field1,field2",
            Serialize(firstRequest)
        };

        var emptyRequest = new StubRequestJson();

        yield return new object[]
        {
            new Uri("https://some.crm4.dynamics.com", UriKind.Absolute),
            new DataverseEntityUpdateIn<StubRequestJson>(
                entityPluralName: "SomeEntities",
                entityKey: new StubEntityKey("SomeKey"),
                selectFields: new[] { string.Empty, "field 1" },
                entityData: emptyRequest),
            "https://some.crm4.dynamics.com/api/data/v9.0/SomeEntities(SomeKey)?$select=field 1",
            Serialize(emptyRequest)
        };

        yield return new object[]
        {
            new Uri("http://ggroupp.ru", UriKind.Absolute),
            new DataverseEntityUpdateIn<StubRequestJson>(
                entityPluralName: "Some/Entities",
                entityKey: new StubEntityKey("Some Key"),
                selectFields: default,
                entityData: emptyRequest),
            "http://ggroupp.ru/api/data/v9.0/Some%2fEntities(Some Key)",
            Serialize(emptyRequest)
        };
    }
}