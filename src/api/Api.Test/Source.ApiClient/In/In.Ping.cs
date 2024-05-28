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
                new(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/WhoAmI",
                    headers: default,
                    content: default)
            },
            {
                new("23f5ed72-b140-4022-a05f-54366719828f"),
                new(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/WhoAmI",
                    headers:
                    [
                        CreateCallerIdHeader("23f5ed72-b140-4022-a05f-54366719828f")
                    ],
                    content: default)
            }
        };
}