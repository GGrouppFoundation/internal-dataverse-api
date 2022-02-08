using System.Collections.Generic;
using System.Net.Http;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetUnitOutputTestData()
        =>
        new[]
        {
            new object?[]
            {
                new StringContent("Some string")
            },
            new object?[]
            {
                null
            }
        };
}