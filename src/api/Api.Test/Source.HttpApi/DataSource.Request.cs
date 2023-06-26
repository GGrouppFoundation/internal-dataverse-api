using System;
using System.Collections.Generic;
using System.Net.Http;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static IEnumerable<object[]> RequestTestData
    {
        get
        {
            yield return new object[]
            {
                new Uri("https://some.crm4.dynamics.com/", UriKind.Absolute),
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Get,
                    url: "/some/data?$select=field1,field2",
                    headers: default,
                    content: default),
                new StubHttpRequestData(HttpMethod.Get, "https://some.crm4.dynamics.com/some/data?$select=field1,field2")
                {
                    Headers = default,
                    Content = default
                }
            };

            var secondRequestJson = new StubRequestJson
            {
                Id = 11,
                Name = "Some name"
            };

            var secondContentJson = secondRequestJson.Serialize();

            yield return new object[]
            {
                new Uri("https://some.crm4.dynamics.com/", UriKind.Absolute),
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Post,
                    url: "http://site.com/some/data?$select=field1,field2&$filter=a eq 1",
                    headers: default,
                    content: new(secondRequestJson)),
                new StubHttpRequestData(HttpMethod.Post, "http://site.com/some/data?$select=field1,field2&$filter=a eq 1")
                {
                    Headers = new(
                        new("Content-Type", "application/json; charset=utf-8"),
                        new("Content-Length", secondContentJson.Length.ToString())),
                    Content = secondContentJson
                }
            };

            var thirdRequestJson = new StubRequestJson();
            var thirdContentJson = thirdRequestJson.Serialize();

            yield return new object[]
            {
                new Uri("https://some.crm4.dynamics.com/", UriKind.Absolute),
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Patch,
                    url: "some/data",
                    headers: new(
                        new("first", "one "),
                        new("second", "two"),
                        new("first", "three,fourth")),
                    content: new(thirdRequestJson)),
                new StubHttpRequestData(HttpMethod.Patch, "https://some.crm4.dynamics.com/some/data")
                {
                    Headers = new(
                        new("first", "one ,three,fourth"),
                        new("second", "two"),
                        new("Content-Type", "application/json; charset=utf-8"),
                        new("Content-Length", thirdContentJson.Length.ToString())),
                    Content = thirdContentJson
                }
            };

            yield return new object[]
            {
                new Uri("http://some.crm4.dynamics.com/", UriKind.Absolute),
                new DataverseHttpRequest<StubRequestJson>(
                    verb: DataverseHttpVerb.Delete,
                    url: "/some/data?$select=field 1,field 2#1",
                    headers: new(
                        new(" first ", "one"),
                        new("second", string.Empty),
                        new("third", " three ")),
                    content: default),
                new StubHttpRequestData(HttpMethod.Delete, "http://some.crm4.dynamics.com/some/data?$select=field 1,field 2#1")
                {
                    Headers = new(
                        new("first", "one"),
                        new("second", string.Empty),
                        new("third", " three ")),
                    Content = default
                }
            };
        }
    }
}