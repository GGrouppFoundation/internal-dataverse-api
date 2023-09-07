using System.Collections.Generic;
using System.Net.Http;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static IEnumerable<object?[]> UnitJsonSuccessTestData
        =>
        new[]
        {
            new object?[]
            {
                new StringContent("Some string")
            },
            new object?[]
            {
                new StringContent(string.Empty)
            },
            new object?[]
            {
                null
            }
        };
}