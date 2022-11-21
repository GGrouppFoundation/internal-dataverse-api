using System;
using System.Collections.Generic;
using System.Net.Http;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetSearchOutputTestData()
    {
        yield return new object?[]
        {
            null,
            new DataverseSearchOut(default, default)
        };

        yield return new object?[]
        {
            new StringContent(string.Empty),
            new DataverseSearchOut(default, default)
        };

        yield return new object?[]
        {
            new StubSearchJsonOut
            {
                TotalRecordCount = 11,
                Value = new[]
                {
                    new StubSearchJsonItem
                    {
                        SearchScore = 151.91,
                        EntityName = "First entity name",
                        ObjectId = "870c75fa-fbaa-4678-875f-062050f3812c",
                        ExtensionData = default
                    },
                    new StubSearchJsonItem
                    {
                        ObjectId = "38927590-1799-4ded-b922-a2cf38033c38"
                    }
                }
            }
            .CreateResponseContentJson(),
            new DataverseSearchOut(
                totalRecordCount: 11,
                value: new(
                    new DataverseSearchItem(
                        searchScore: 151.91,
                        objectId: Guid.Parse("870c75fa-fbaa-4678-875f-062050f3812c"),
                        entityName: "First entity name",
                        extensionData: default),
                    new DataverseSearchItem(
                        searchScore: default,
                        objectId: Guid.Parse("38927590-1799-4ded-b922-a2cf38033c38"),
                        entityName: string.Empty,
                        extensionData: default)))
        };
    }
}