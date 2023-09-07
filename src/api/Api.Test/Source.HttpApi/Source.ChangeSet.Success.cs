using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static IEnumerable<object[]> ChangeSetSuccessTestData
        =>
        new[]
        {
            new object[]
            {
                new MultipartContent("mixed", "batch_GGJH123GJ"),
                default(DataverseChangeSetResponse)
            },
            new object[]
            {
                new MultipartContent("mixed", "batch_ajhsgd191ka1")
                {
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent("Some conetnt")
                    }
                    .ToMessageContent(),
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent("Second conetnt")
                    }
                    .ToMessageContent(),
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Created,
                        Content = null
                    }
                    .ToMessageContent(),
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Created,
                        Content = new StringContent("The Fourth")
                    }
                    .ToMessageContent()
                },
                new DataverseChangeSetResponse(
                    responses: new DataverseJsonResponse[]
                    {
                        default,
                        new(new("Second conetnt")),
                        default,
                        new(new("The Fourth"))
                    })
            }
        };
}