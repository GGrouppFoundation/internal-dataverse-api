using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetEntitySetGetTestDataPair()
        =>
        new[]
        {
            new object?[]
            {
                new Uri("https://some.crm4.dynamics.com/", UriKind.Absolute),
                new DataverseEntitySetGetIn(
                    entityPluralName: "SomeEntities",
                    selectFields: new[] { "field1", "field2" },
                    filter: "id eq 15",
                    orderBy: new DataverseOrderParameter[]
                    {
                        new("field3", DataverseOrderDirection.Descending),
                        new("field4"),
                        new("field5", DataverseOrderDirection.Ascending)
                    },
                    top: 15),
                "https://some.crm4.dynamics.com/api/data/v9.0/SomeEntities?$select=field1,field2&$filter=id eq 15&$orderby=field3 desc,field4,field5 asc&$top=15",
                null
            },
            new object?[]
            {
                new Uri("https://some.crm4.dynamics.com/", UriKind.Absolute),
                new DataverseEntitySetGetIn(
                    entityPluralName: "Some Entities",
                    selectFields: new[] { string.Empty, "field 1" },
                    filter: default,
                    orderBy: default,
                    top: 1)
                    {
                        IncludeAnnotations = "display.*"
                    },
                "https://some.crm4.dynamics.com/api/data/v9.0/Some+Entities?$select=field 1&$top=1",
                "odata.include-annotations=display.*"
            },
            new object?[]
            {
                new Uri("http://ggroupp.ru", UriKind.Absolute),
                new DataverseEntitySetGetIn(
                    entityPluralName: "Some/Entities",
                    selectFields: default,
                    filter: "date gt 2020-01-01")
                    {
                        IncludeAnnotations = "*"
                    },
                "http://ggroupp.ru/api/data/v9.0/Some%2fEntities?$filter=date gt 2020-01-01",
                "odata.include-annotations=*"
            }
        };
}