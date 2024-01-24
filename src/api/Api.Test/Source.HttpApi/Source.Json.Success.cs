using System.Net.Http;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static TheoryData<StringContent?, DataverseJsonResponse> JsonSuccessTestData
        =>
        new()
        {
            {
                null,
                default
            },
            {
                new(string.Empty),
                default
            },
            {
                new("Some text"),
                new(
                    content: new("Some text"))
            },
            {
                new StubResponseJson
                {
                    Id = 15,
                    Name = "Some name"
                }
                .CreateResponseContentJson(),
                new(
                    content: new(
                        new StubResponseJson
                        {
                            Id = 15,
                            Name = "Some name"
                        }
                        .Serialize()))
            }
        };
}