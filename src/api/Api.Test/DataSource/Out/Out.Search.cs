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

        var firstItem = new DataverseSearchJsonItem
        {
            SearchScore = 151.91,
            EntityName = "First entity name",
            ObjectId = Guid.Parse("870c75fa-fbaa-4678-875f-062050f3812c"),
            ExtensionData = default
        };

        var secondItem = new DataverseSearchJsonItem
        {
            ObjectId = Guid.Parse("38927590-1799-4ded-b922-a2cf38033c38")
        };

        var responseJson = new DataverseSearchJsonOut
        {
            TotalRecordCount = 11,
            Value = new[] { firstItem, secondItem }
        };

        var expected = new DataverseSearchOut(
            totalRecordCount: responseJson.TotalRecordCount,
            value: new DataverseSearchItem[]
            {
                new(
                    searchScore: firstItem.SearchScore,
                    objectId: firstItem.ObjectId,
                    entityName: firstItem.EntityName,
                    extensionData: default),
                new(
                    searchScore: default,
                    objectId: secondItem.ObjectId,
                    entityName: string.Empty,
                    extensionData: default)
            });

        yield return new object?[]
        {
            CreateResponseContentJson(responseJson),
            expected
        };
    }
}