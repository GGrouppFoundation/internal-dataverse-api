using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> SearchInputTestData
        =>
        new[]
        {
            new object?[]
            {
                null,
                new DataverseSearchIn("Some first text")
                {
                    OrderBy = new("field 1"),
                    Top = 10,
                    Skip = 5,
                    ReturnTotalRecordCount = false,
                    Filter = "Some filter",
                    Facets = new("one", "two", string.Empty),
                    Entities = new("first", "second", string.Empty, "third"),
                    SearchMode = DataverseSearchMode.Any,
                    SearchType = DataverseSearchType.Full
                },
                new DataverseHttpRequest<DataverseSearchJsonIn>(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/search/v1.0/query",
                    headers: default,
                    content: new DataverseSearchJsonIn
                    {
                        Search = "Some first text",
                        OrderBy = new[] { "field 1" },
                        Top = 10,
                        Skip = 5,
                        ReturnTotalRecordCount = false,
                        Filter = "Some filter",
                        Facets = new[] { "one", "two" },
                        Entities = new[] { "first", "second", "third" },
                        SearchMode = DataverseSearchModeJson.Any,
                        SearchType = DataverseSearchTypeJson.Full
                    })
            },
            new object?[]
            {
                Guid.Parse("aa087335-0897-4d6e-82cb-0f07cb6fc2f4"),
                new DataverseSearchIn("Some second text")
                {
                    OrderBy = new("field 1", string.Empty),
                    SearchMode = DataverseSearchMode.All,
                    SearchType = DataverseSearchType.Simple
                },
                new DataverseHttpRequest<DataverseSearchJsonIn>(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/search/v1.0/query",
                    headers: new(
                        CreateCallerIdHeader("aa087335-0897-4d6e-82cb-0f07cb6fc2f4")),
                    content: new DataverseSearchJsonIn
                    {
                        Search = "Some second text",
                        OrderBy = new[] { "field 1" },
                        SearchMode = DataverseSearchModeJson.All,
                        SearchType = DataverseSearchTypeJson.Simple
                    })
            },
            new object?[]
            {
                null,
                new DataverseSearchIn(string.Empty),
                new DataverseHttpRequest<DataverseSearchJsonIn>(
                    verb: DataverseHttpVerb.Post,
                    url: "/api/search/v1.0/query",
                    headers: default,
                    content: new DataverseSearchJsonIn
                    {
                        Search = string.Empty
                    })
            }
        };
}