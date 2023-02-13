using System.Collections.Generic;
using System.Net.Http;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static IEnumerable<object?[]> UnitSuccessTestData
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