using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object[]> GetSearchInputTestData()
    {
        yield return new object[]
        {
            new Uri("https://some.crm4.dynamics.com/", UriKind.Absolute),
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
            "https://some.crm4.dynamics.com/api/search/v1.0/query",
            new StubSearchJsonIn
            {
                Search = "Some first text",
                OrderBy = new[] { "field 1" },
                Top = 10,
                Skip = 5,
                ReturnTotalRecordCount = false,
                Filter = "Some filter",
                Facets = new[] { "one", "two" },
                Entities = new[] { "first", "second", "third" },
                SearchMode = 0,
                SearchType = 1
            }
            .Serialize()
        };

        yield return new object[]
        {
            new Uri("https://some.crm4.dynamics.com", UriKind.Absolute),
            new DataverseSearchIn("Some second text")
            {
                OrderBy = new("field 1", string.Empty),
                SearchMode = DataverseSearchMode.All,
                SearchType = DataverseSearchType.Simple
            },
            "https://some.crm4.dynamics.com/api/search/v1.0/query",
            new StubSearchJsonIn
            {
                Search = "Some second text",
                OrderBy = new[] { "field 1" },
                SearchMode = 1,
                SearchType = 0
            }
            .Serialize()
        };

        yield return new object[]
        {
            new Uri("http://ggroupp.ru", UriKind.Absolute),
            new DataverseSearchIn(string.Empty),
            "http://ggroupp.ru/api/search/v1.0/query",
            new StubSearchJsonIn
            {
                Search = string.Empty
            }
            .Serialize()
        };
    }
}