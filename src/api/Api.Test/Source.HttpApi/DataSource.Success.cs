using System.Collections.Generic;
using System.Net.Http;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static IEnumerable<object?[]> SuccessTestData
    {
        get
        {
            yield return new object?[]
            {
                null,
                default(StubResponseJson)
            };

            yield return new object?[]
            {
                new StringContent(string.Empty),
                default(StubResponseJson)
            };

            yield return new object?[]
            {
                new StubResponseJson
                {
                    Id = 15,
                    Name = "Some name"
                }
                .CreateResponseContentJson(),
                new StubResponseJson
                {
                    Id = 15,
                    Name = "Some name"
                }
            };
        }
    }
}