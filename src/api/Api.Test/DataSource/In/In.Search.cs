using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object[]> GetSearchInputTestData()
    {
        var firstRequest = new DataverseSearchIn("Some first text")
        {
            OrderBy = new[] { "field 1" },
            Top = 10,
            Skip = 5,
            ReturnTotalRecordCount = false,
            Filter = "Some filter",
            Facets = new string[] { "one", "two" },
            Entities = new string[] { "first", "second", "third" },
            SearchMode = DataverseSearchMode.Any,
            SearchType = DataverseSearchType.Simple
        };

        var firstExpected = new DataverseSearchJsonIn(firstRequest.Search)
        {
            OrderBy = firstRequest.OrderBy,
            Top = firstRequest.Top,
            Skip = firstRequest.Skip,
            ReturnTotalRecordCount = firstRequest.ReturnTotalRecordCount,
            Filter = firstRequest.Filter,
            Facets = firstRequest.Facets,
            Entities = firstRequest.Entities,
            SearchMode = DataverseSearchModeJson.Any,
            SearchType = DataverseSearchTypeJson.Simple
        };

        yield return new object[]
        {
            new Uri("https://some.crm4.dynamics.com/", UriKind.Absolute),
            firstRequest,
            "https://some.crm4.dynamics.com/api/search/v1.0/query",
            Serialize(firstExpected)
        };

        var secondRequest = new DataverseSearchIn("Some second text")
        {
            OrderBy = new[] { "field 1", string.Empty },
            SearchMode = DataverseSearchMode.All,
            SearchType = DataverseSearchType.Full
        };

        var secondExpected = new DataverseSearchJsonIn(secondRequest.Search)
        {
            OrderBy = secondRequest.OrderBy,
            SearchMode = DataverseSearchModeJson.All,
            SearchType = DataverseSearchTypeJson.Full
        };

        yield return new object[]
        {
            new Uri("https://some.crm4.dynamics.com", UriKind.Absolute),
            secondRequest,
            "https://some.crm4.dynamics.com/api/search/v1.0/query",
            Serialize(secondExpected)
        };

        var emptyRequest = new DataverseSearchIn(string.Empty);
        var emptyExpected = new DataverseSearchJsonIn(string.Empty);

        yield return new object[]
        {
            new Uri("http://ggroupp.ru", UriKind.Absolute),
            emptyRequest,
            "http://ggroupp.ru/api/search/v1.0/query",
            Serialize(emptyExpected)
        };
    }
}