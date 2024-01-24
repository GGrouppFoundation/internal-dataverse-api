using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static TheoryData<Uri, DataverseChangeSetRequest, StubHttpRequestData> ChangeSetRequestTestData
    {
        get
        {
            const string emptyChangeSetContent
                =
                "--batch_7a5584dd-0b9a-4439-b4a8-594eb84378a2" +
                "\r\nContent-Type: multipart/mixed; boundary=\"changeset_3a8b6f47-98a1-42b7-8a04-cb28aeabae9c\"" +
                "\r\n\r\n--changeset_3a8b6f47-98a1-42b7-8a04-cb28aeabae9c" +
                "\r\n\r\n--changeset_3a8b6f47-98a1-42b7-8a04-cb28aeabae9c--" +
                "\r\n\r\n--batch_7a5584dd-0b9a-4439-b4a8-594eb84378a2--" +
                "\r\n";

            var data = new TheoryData<Uri, DataverseChangeSetRequest, StubHttpRequestData>
            {
                {
                    new("https://some.crm4.dynamics.com/", UriKind.Absolute),
                    new(
                        url: "/api/$batch",
                        batchId: new("7a5584dd-0b9a-4439-b4a8-594eb84378a2"),
                        changeSetId: new("3a8b6f47-98a1-42b7-8a04-cb28aeabae9c"),
                        headers: default,
                        requests: default),
                    new(HttpMethod.Post, "https://some.crm4.dynamics.com/api/$batch")
                    {
                        Headers = new KeyValuePair<string, string>[]
                        {
                            new("Content-Type", "multipart/mixed; boundary=\"batch_7a5584dd-0b9a-4439-b4a8-594eb84378a2\""),
                            new("Content-Length", emptyChangeSetContent.Length.ToString())
                        },
                        Content = emptyChangeSetContent
                    }
                }
            };

            var secondJsonContent = new StubRequestJson
            {
                Id = 11,
                Name = "Some name"
            }
            .Serialize();

            var secondChangeSetContent
                =
                "--batch_3d3a0ba3-6533-495d-a1e4-bdaa14b593f4" +
                "\r\nContent-Type: multipart/mixed; boundary=\"changeset_c57e60f8-3133-415f-8ade-93993d63b91e\"" +
                "\r\n\r\n--changeset_c57e60f8-3133-415f-8ade-93993d63b91e" +
                "\r\nContent-Type: application/http" +
                "\r\nContent-Transfer-Encoding: binary" +
                "\r\nContent-ID: 1" +
                "\r\n\r\nPOST /api/data/v9.2/contacts?$select=contactid HTTP/1.1" +
                "\r\nheader1: value1" +
                "\r\nContent-Type: application/json; type=entry" +
                "\r\n\r\n" + secondJsonContent +
                "\r\n--changeset_c57e60f8-3133-415f-8ade-93993d63b91e--" +
                "\r\n\r\n--batch_3d3a0ba3-6533-495d-a1e4-bdaa14b593f4--" +
                "\r\n";

            data.Add(
                new("https://some.crm4.dynamics.com/", UriKind.Absolute),
                new(
                    url: "/api/$batch?f=1",
                    batchId: new("3d3a0ba3-6533-495d-a1e4-bdaa14b593f4"),
                    changeSetId: new("c57e60f8-3133-415f-8ade-93993d63b91e"),
                    headers: new DataverseHttpHeader[]
                    {
                        new("first", "one "),
                        new("second", "two"),
                        new("first", "three,fourth")
                    },
                    requests: new DataverseJsonRequest[]
                    {
                        new(
                            verb: DataverseHttpVerb.Post,
                            url: "/api/data/v9.2/contacts?$select=contactid",
                            headers: new DataverseHttpHeader[]
                            {
                                new("content-Type", "application/json"),
                                new("header1", "value1")
                            },
                            content: new(secondJsonContent))
                    }),
                new(HttpMethod.Post, "https://some.crm4.dynamics.com/api/$batch?f=1")
                {
                    Headers = new KeyValuePair<string, string>[]
                    {
                        new("first", "one ,three,fourth"),
                        new("second", "two"),
                        new("Content-Type", "multipart/mixed; boundary=\"batch_3d3a0ba3-6533-495d-a1e4-bdaa14b593f4\""),
                        new("Content-Length", secondChangeSetContent.Length.ToString())
                    },
                    Content = secondChangeSetContent
                });

            const string thirdChangeSetContent
                =
                "--batch_0f595005-0d61-4fda-9071-321fdcdba6a2" +
                "\r\nContent-Type: multipart/mixed; boundary=\"changeset_12b2176b-1a84-4b5e-89e9-277764ad074b\"" +
                "\r\n\r\n--changeset_12b2176b-1a84-4b5e-89e9-277764ad074b" +
                "\r\nContent-Type: application/http" +
                "\r\nContent-Transfer-Encoding: binary" +
                "\r\nContent-ID: 1" +
                "\r\n\r\nPATCH api/data/v9.2/contacts HTTP/1.1" +
                "\r\n" +
                "\r\n\r\n--changeset_12b2176b-1a84-4b5e-89e9-277764ad074b" +
                "\r\nContent-Type: application/http" +
                "\r\nContent-Transfer-Encoding: binary" +
                "\r\nContent-ID: 2" +
                "\r\n\r\nDELETE /api/contacts(00d425fa-5054-4d3f-9e56-93a8a41b70f5)?v=1 HTTP/1.1" +
                "\r\nfirst: one" +
                "\r\nContent-Type: application/json; type=entry" +
                "\r\n\r\nSome Json content" +
                "\r\n--changeset_12b2176b-1a84-4b5e-89e9-277764ad074b--" +
                "\r\n\r\n--batch_0f595005-0d61-4fda-9071-321fdcdba6a2--" +
                "\r\n";

            data.Add(
                new("https://some.crm4.dynamics.com/", UriKind.Absolute),
                new(
                    url: "/api/batch",
                    batchId: new("0f595005-0d61-4fda-9071-321fdcdba6a2"),
                    changeSetId: new("12b2176b-1a84-4b5e-89e9-277764ad074b"),
                    headers: default,
                    requests: new DataverseJsonRequest[]
                    {
                        new(
                            verb: DataverseHttpVerb.Patch,
                            url: "api/data/v9.2/contacts",
                            headers: default,
                            content: default),
                        new(
                            verb: DataverseHttpVerb.Delete,
                            url: "/api/contacts(00d425fa-5054-4d3f-9e56-93a8a41b70f5)?v=1",
                            headers: new DataverseHttpHeader[]
                            {
                                new("first", "one")
                            },
                            content: new("Some Json content"))
                    }),
                new(HttpMethod.Post, "https://some.crm4.dynamics.com/api/batch")
                {
                    Headers = new KeyValuePair<string, string>[]
                    {
                        new("Content-Type", "multipart/mixed; boundary=\"batch_0f595005-0d61-4fda-9071-321fdcdba6a2\""),
                        new("Content-Length", thirdChangeSetContent.Length.ToString())
                    },
                    Content = thirdChangeSetContent
                });

            return data;
        }
    }
}