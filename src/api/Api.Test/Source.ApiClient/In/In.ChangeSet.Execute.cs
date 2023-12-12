using System;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<Guid?, Guid, Guid, DataverseChangeSetExecuteIn<object>, DataverseChangeSetRequest> ChangeSetExecuteInputTestData
        =>
        new()
        {
            {
                Guid.Parse("947700e6-39fd-411a-a6f8-a39300416985"),
                Guid.Parse("894354d2-9669-4b19-9998-ae704c68f2b9"),
                Guid.Parse("08ad1bcd-50c1-4fb7-bbe9-ff315a5fcf8f"),
                new(
                    requests: new IDataverseTransactableIn<object>[]
                    {
                        new DataverseEntityUpdateIn<StubRequestJson>(
                            entityPluralName: "SomeEntities",
                            entityKey: new StubEntityKey("SomeKey"),
                            entityData: new()
                            {
                                Id = 101,
                                Name = "First request name"
                            })
                        {
                            SuppressDuplicateDetection = true
                        }
                    }),
                new(
                    url: "/api/data/v9.2/$batch",
                    batchId: Guid.Parse("894354d2-9669-4b19-9998-ae704c68f2b9"),
                    changeSetId: Guid.Parse("08ad1bcd-50c1-4fb7-bbe9-ff315a5fcf8f"),
                    headers: CreateCallerIdHeader("947700e6-39fd-411a-a6f8-a39300416985").AsFlatArray(),
                    requests: new DataverseJsonRequest[]
                    {
                        new(
                            verb: DataverseHttpVerb.Patch,
                            url: "/api/data/v9.2/SomeEntities(SomeKey)",
                            headers: new(
                                CreateCallerIdHeader("947700e6-39fd-411a-a6f8-a39300416985"),
                                CreateSuppressDuplicateDetectionHeader("true")),
                            content: new StubRequestJson
                            {
                                Id = 101,
                                Name = "First request name"
                            }.InnerToJsonContentIn())
                    })
            },
            {
                null,
                Guid.Parse("90ca0c0f-caf8-44b5-aa01-034da39d0953"),
                Guid.Parse("97a5109b-347b-4ea7-bd3a-790cfed94268"),
                new(
                    requests: new IDataverseTransactableIn<object>[]
                    {
                        new DataverseEntityDeleteIn(
                            entityPluralName: "Some/Entities",
                            entityKey: new StubEntityKey("Some=Key")),
                        new DataverseEntityCreateIn<StubRequestJson>(
                            entityPluralName: "SomeEntities",
                            entityData:  new()
                            {
                                Id = 17,
                                Name = "First request name"
                            })
                    }),
                new(
                    url: "/api/data/v9.2/$batch",
                    batchId: Guid.Parse("90ca0c0f-caf8-44b5-aa01-034da39d0953"),
                    changeSetId: Guid.Parse("97a5109b-347b-4ea7-bd3a-790cfed94268"),
                    headers: default,
                    requests: new DataverseJsonRequest[]
                    {
                        new(
                            verb: DataverseHttpVerb.Delete,
                            url: "/api/data/v9.2/Some%2fEntities(Some=Key)",
                            headers: default,
                            content: default),
                        new(
                            verb: DataverseHttpVerb.Post,
                            url: "/api/data/v9.2/SomeEntities",
                            headers: default,
                            content: new StubRequestJson
                            {
                                Id = 17,
                                Name = "First request name"
                            }.InnerToJsonContentIn())
                    })
            }
        };
}