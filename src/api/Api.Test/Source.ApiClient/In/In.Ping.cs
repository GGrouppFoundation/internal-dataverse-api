using System;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<Guid?, DataverseJsonRequest> PingInputTestData
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
                Guid.Parse("23f5ed72-b140-4022-a05f-54366719828f"),
                new DataverseJsonRequest(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/WhoAmI",
                    headers: new(
                        CreateCallerIdHeader("23f5ed72-b140-4022-a05f-54366719828f")),
                    content: default)
            }
        };
}