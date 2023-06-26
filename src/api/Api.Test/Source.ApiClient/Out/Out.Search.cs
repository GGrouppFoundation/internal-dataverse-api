using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> SearchOutputTestData
        =>
        new[]
        {
            new object?[]
            {
                new DataverseSearchJsonOut(),
                new DataverseSearchOut(default, default)
            },
            new object?[]
            {
                new DataverseSearchJsonOut
                {
                    TotalRecordCount = 11,
                    Value = new(
                        new()
                        {
                            SearchScore = 151.91,
                            EntityName = "First entity name",
                            ObjectId = Guid.Parse("870c75fa-fbaa-4678-875f-062050f3812c"),
                            ExtensionData = default
                        },
                        new()
                        {
                            ObjectId = Guid.Parse("38927590-1799-4ded-b922-a2cf38033c38")
                        })
                },
                new DataverseSearchOut(
                    totalRecordCount: 11,
                    value: new(
                        new(
                            searchScore: 151.91,
                            objectId: Guid.Parse("870c75fa-fbaa-4678-875f-062050f3812c"),
                            entityName: "First entity name",
                            extensionData: default),
                        new(
                            searchScore: default,
                            objectId: Guid.Parse("38927590-1799-4ded-b922-a2cf38033c38"),
                            entityName: string.Empty,
                            extensionData: default)))
            }
        };
}