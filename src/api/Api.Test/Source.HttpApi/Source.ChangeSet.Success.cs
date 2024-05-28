using System.Net;
using System.Net.Http;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static TheoryData<MultipartContent, DataverseChangeSetResponse> ChangeSetSuccessTestData
        =>
        new()
        {
            {
                new("mixed", "batch_GGJH123GJ"),
                default
            },
            {
                new("mixed", "batch_ajhsgd191ka1")
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
                new(
                    responses:
                    [
                        default,
                        new(new("Second conetnt")),
                        default,
                        new(new("The Fourth"))
                    ])
            }
        };
}