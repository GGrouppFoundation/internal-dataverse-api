using System;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<Guid?, Guid, Guid, DataverseChangeSetExecuteIn<object>, DataverseChangeSetRequest> ChangeSetExecuteWithTOutInputTestData
        =>
        new()
        {
            {
                null,
                Guid.Parse("ce1a0575-e83e-4b5f-865a-8d501df4e40c"),
                Guid.Parse("d7762c7e-7774-4771-a5dc-64578840a210"),
                new(
                    requests: new IDataverseTransactableIn<object>[]
                    {
                        new DataverseEntityCreateIn<StubRequestJson>(
                            entityPluralName: "SomeEntities",
                            selectFields: new[] { string.Empty, "field 1" },
                            entityData: new())
                        {
                            ExpandFields = new DataverseExpandedField[]
                            {
                                new("LookupOne", new("field1.1", "field1.2")),
                                new(
                                    fieldName: "LookupTwo",
                                    selectFields: default,
                                    expandFields: new DataverseExpandedField[]
                                    {
                                        new("field2.1", default)
                                    })
                            },
                            SuppressDuplicateDetection = false
                        }
                    }),
                new(
                    url: "/api/data/v9.2/$batch",
                    batchId: Guid.Parse("ce1a0575-e83e-4b5f-865a-8d501df4e40c"),
                    changeSetId: Guid.Parse("d7762c7e-7774-4771-a5dc-64578840a210"),
                    headers: default,
                    requests: new DataverseJsonRequest[]
                    {
                        new(
                            verb: DataverseHttpVerb.Post,
                            url: "/api/data/v9.2/SomeEntities?$select=field 1" +
                                "&$expand=LookupOne($select=field1.1,field1.2),LookupTwo($expand=field2.1)",
                            headers: new(
                                PreferRepresentationHeader,
                                CreateSuppressDuplicateDetectionHeader("false")),
                        content: new StubRequestJson().InnerToJsonContentIn())
                    })
            },
            {
                Guid.Parse("e97607f1-8716-4efc-947c-c631d81be853"),
                Guid.Parse("1f23da9e-be1b-4967-9aff-f0723876d275"),
                Guid.Parse("f2882787-a456-4ff5-bb13-87fdb377849f"),
                new(
                    requests: new IDataverseTransactableIn<object>[]
                    {
                        new DataverseEntityUpdateIn<StubRequestJson>(
                            entityPluralName: "Some/Entities",
                            entityKey: new StubEntityKey("Some Key"),
                            selectFields: default,
                            entityData: new())
                        {
                            ExpandFields = new DataverseExpandedField[]
                            {
                                new("Field1", new("Lookup Field"))
                            }
                        },
                        new DataverseEntityDeleteIn(
                            entityPluralName: "SomeEntities",
                            entityKey: new StubEntityKey("SomeKey"))
                    }),
                new(
                    url: "/api/data/v9.2/$batch",
                    batchId: Guid.Parse("1f23da9e-be1b-4967-9aff-f0723876d275"),
                    changeSetId: Guid.Parse("f2882787-a456-4ff5-bb13-87fdb377849f"),
                    headers: CreateCallerIdHeader("e97607f1-8716-4efc-947c-c631d81be853").AsFlatArray(),
                    requests: new DataverseJsonRequest[]
                    {
                        new(
                            verb: DataverseHttpVerb.Patch,
                            url: "/api/data/v9.2/Some%2fEntities(Some Key)?$expand=Field1($select=Lookup Field)",
                            headers: new(
                                CreateCallerIdHeader("e97607f1-8716-4efc-947c-c631d81be853"),
                                PreferRepresentationHeader),
                            content: new StubRequestJson().InnerToJsonContentIn()),
                        new(
                            verb: DataverseHttpVerb.Delete,
                            url: "/api/data/v9.2/SomeEntities(SomeKey)",
                            headers: new(
                                CreateCallerIdHeader("e97607f1-8716-4efc-947c-c631d81be853")),
                            content: default)
                    })
            }
        };
}