using System;
using System.Collections.Generic;
using System.Net.Http;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetWhoAmIOutputTestData()
    {
        yield return new object?[]
        {
            null,
            default(DataverseWhoAmIOut)
        };

        yield return new object?[]
        {
            new StringContent(string.Empty),
            default(DataverseWhoAmIOut)
        };

        yield return new object?[]
        {
            new StubWhoAmIOutJson
            {
                BusinessUnitId = "51ea96d6-5119-4059-b649-d90c0a4aeab6",
                UserId = "6ac49276-357d-441f-89c4-c118cbaa5ee3",
                OrganizationId = "92847989-5772-4ff9-851b-661dc66720ae"
            }
            .CreateResponseContentJson(),
            new DataverseWhoAmIOut(
                businessUnitId: Guid.Parse("51ea96d6-5119-4059-b649-d90c0a4aeab6"),
                userId: Guid.Parse("6ac49276-357d-441f-89c4-c118cbaa5ee3"),
                organizationId: Guid.Parse("92847989-5772-4ff9-851b-661dc66720ae"))
        };
    }
}