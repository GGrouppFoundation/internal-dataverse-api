using System.Collections.Generic;
using System.Net.Http;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static IEnumerable<object?[]> JsonSuccessTestData
    {
        get
        {
            yield return new object?[]
            {
                null,
                default(DataverseJsonResponse)
            };

            yield return new object?[]
            {
                new StringContent(string.Empty),
                default(DataverseJsonResponse)
            };

            yield return new object?[]
            {
                new StringContent("Some text"),
                new DataverseJsonResponse(
                    content: new("Some text"))
            };

            yield return new object?[]
            {
                new StubResponseJson
                {
                    Id = 15,
                    Name = "Some name"
                }
                .CreateResponseContentJson(),
                new DataverseJsonResponse(
                    content: new(
                        new StubResponseJson
                        {
                            Id = 15,
                            Name = "Some name"
                        }
                        .Serialize()))
            };
        }
    }
}