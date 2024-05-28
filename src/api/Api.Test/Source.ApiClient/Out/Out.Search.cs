using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<DataverseSearchJsonOut, DataverseSearchOut> SearchOutputTestData
        =>
        new()
        {
            {
                new(),
                new(default, default)
            },
            {
                new()
                {
                    TotalRecordCount = 11,
                    Value =
                    [
                        new()
                        {
                            SearchScore = 151.91,
                            EntityName = "First entity name",
                            ObjectId = new("870c75fa-fbaa-4678-875f-062050f3812c"),
                            ExtensionData = default
                        },
                        new()
                        {
                            ObjectId = new("38927590-1799-4ded-b922-a2cf38033c38")
                        }
                    ]
                },
                new(
                    totalRecordCount: 11,
                    value:
                    [
                        new(
                            searchScore: 151.91,
                            objectId: new("870c75fa-fbaa-4678-875f-062050f3812c"),
                            entityName: "First entity name",
                            extensionData: default),
                        new(
                            searchScore: default,
                            objectId: new("38927590-1799-4ded-b922-a2cf38033c38"),
                            entityName: string.Empty,
                            extensionData: default)
                    ])
            }
        };
}