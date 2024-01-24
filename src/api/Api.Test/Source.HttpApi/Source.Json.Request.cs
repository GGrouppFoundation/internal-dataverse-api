using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static TheoryData<Uri, DataverseJsonRequest, StubHttpRequestData> JsonRequestTestData
    {
        get
        {
            var data = new TheoryData<Uri, DataverseJsonRequest, StubHttpRequestData>
            {
                {
                    new("https://some.crm4.dynamics.com/", UriKind.Absolute),
                    new(
                        verb: DataverseHttpVerb.Get,
                        url: "/some/data?$select=field1,field2",
                        headers: default,
                        content: default),
                    new(HttpMethod.Get, "https://some.crm4.dynamics.com/some/data?$select=field1,field2")
                    {
                        Headers = default,
                        Content = default
                    }
                }
            };

            var secondRequestJson = new StubRequestJson
            {
                Id = 11,
                Name = "Some name"
            };

            var secondContentJson = secondRequestJson.Serialize();

            data.Add(
                new("https://some.crm4.dynamics.com/", UriKind.Absolute),
                new(
                    verb: DataverseHttpVerb.Post,
                    url: "http://site.com/some/data?$select=field1,field2&$filter=a eq 1",
                    headers: default,
                    content: new(secondContentJson)),
                new(HttpMethod.Post, "http://site.com/some/data?$select=field1,field2&$filter=a eq 1")
                {
                    Headers = new KeyValuePair<string, string>[]
                    {
                        new("Content-Type", "application/json; charset=utf-8"),
                        new("Content-Length", secondContentJson.Length.ToString())
                    },
                    Content = secondContentJson
                });

            data.Add(
                new("https://some.crm4.dynamics.com/", UriKind.Absolute),
                new(
                    verb: DataverseHttpVerb.Patch,
                    url: "some/data",
                    headers: new DataverseHttpHeader[]
                    {
                        new("first", "one "),
                        new("second", "two"),
                        new("first", "three,fourth")
                    },
                    content: new(string.Empty)),
                new(HttpMethod.Patch, "https://some.crm4.dynamics.com/some/data")
                {
                    Headers = new KeyValuePair<string, string>[]
                    {
                        new("first", "one ,three,fourth"),
                        new("second", "two")
                    },
                    Content = default
                });

            data.Add(
                new("http://some.crm4.dynamics.com/", UriKind.Absolute),
                new(
                    verb: DataverseHttpVerb.Delete,
                    url: "/some/data?$select=field 1,field 2#1",
                    headers: new DataverseHttpHeader[]
                    {
                        new(" first ", "one"),
                        new("second", string.Empty),
                        new("third", " three ")
                    },
                    content: default),
                new(HttpMethod.Delete, "http://some.crm4.dynamics.com/some/data?$select=field 1,field 2#1")
                {
                    Headers = new KeyValuePair<string, string>[]
                    {
                        new("first", "one"),
                        new("second", string.Empty),
                        new("third", " three ")
                    },
                    Content = default
                });

            return data;
        }
    }
}