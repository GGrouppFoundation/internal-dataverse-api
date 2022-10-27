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

        var responseJson = new DataverseWhoAmIOutJson
        {
            BusinessUnitId = Guid.Parse("da7d87f5380e40668c8011356e6d4ba1"),
            UserId = Guid.Parse("6ac49276-357d-441f-89c4-c118cbaa5ee3"),
            OrganizationId = Guid.Parse("92847989-5772-4ff9-851b-661dc66720ae")
        };

        var expected = new DataverseWhoAmIOut(
            businessUnitId: responseJson.BusinessUnitId,
            userId: responseJson.UserId,
            organizationId: responseJson.OrganizationId);

        yield return new object?[]
        {
            CreateResponseContentJson(responseJson),
            expected
        };
    }
}