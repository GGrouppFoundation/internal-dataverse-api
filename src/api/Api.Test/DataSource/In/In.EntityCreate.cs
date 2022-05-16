using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object[]> GetEntityCreateInputTestData()
    {
        var firstRequest = new StubRequestJson
        {
            Id = 17,
            Name = "First request name"
        };

        yield return new object[]
        {
            new Uri("https://some.crm4.dynamics.com/", UriKind.Absolute),
            new DataverseEntityCreateIn<StubRequestJson>(
                entityPluralName: "SomeEntities",
                selectFields: new[] { "field1", "field2" },
                entityData: firstRequest),
            "https://some.crm4.dynamics.com/api/data/v9.2/SomeEntities?$select=field1,field2",
            Serialize(firstRequest)
        };

        var emptyRequest = new StubRequestJson();

        yield return new object[]
        {
            new Uri("https://some.crm4.dynamics.com", UriKind.Absolute),
            new DataverseEntityCreateIn<StubRequestJson>(
                entityPluralName: "SomeEntities",
                selectFields: new[] { string.Empty, "field 1" },
                entityData: emptyRequest),
            "https://some.crm4.dynamics.com/api/data/v9.2/SomeEntities?$select=field 1",
            Serialize(emptyRequest)
        };

        yield return new object[]
        {
            new Uri("http://ggroupp.ru", UriKind.Absolute),
            new DataverseEntityCreateIn<StubRequestJson>(
                entityPluralName: "Some/Entities",
                selectFields: default,
                entityData: emptyRequest),
            "http://ggroupp.ru/api/data/v9.2/Some%2fEntities",
            Serialize(emptyRequest)
        };
    }
}