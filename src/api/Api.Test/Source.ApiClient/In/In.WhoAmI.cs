using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetWhoAmIInputTestData()
        =>
        new[]
        {
            new object?[]
            {
                null,
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/WhoAmI",
                    headers: default,
                    content: default)
            },
            new object?[]
            {
                Guid.Parse("68731777-a43a-454c-bd93-7680a887e6eb"),
                new DataverseHttpRequest<Unit>(
                    verb: DataverseHttpVerb.Get,
                    url: "/api/data/v9.2/WhoAmI",
                    headers: new(
                        CreateCallerIdHeader("68731777-a43a-454c-bd93-7680a887e6eb")),
                    content: default)
            }
        };
}