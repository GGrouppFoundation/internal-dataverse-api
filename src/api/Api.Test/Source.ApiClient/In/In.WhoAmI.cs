using System;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<Guid?, DataverseJsonRequest> WhoAmIInputTestData
        =>
        new()
        {
            {
                null,
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/WhoAmI",
                    headers: default,
                    content: default)
            },
            {
                Guid.Parse("68731777-a43a-454c-bd93-7680a887e6eb"),
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/WhoAmI",
                    headers: new(
                        CreateCallerIdHeader("68731777-a43a-454c-bd93-7680a887e6eb")),
                    content: default)
            }
        };
}