using System;
using AutoFixture;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<DataverseFetchXmlOutJson<StubResponseJson>, DataverseFetchXmlOut<StubResponseJson>> FetchStubResponseJsonTestData
    {
        get
        {
            var data = new TheoryData<DataverseFetchXmlOutJson<StubResponseJson>, DataverseFetchXmlOut<StubResponseJson>>();

            var fixture = new Fixture();
            var rnd = new Random(DateTime.UnixEpoch.Millisecond);

            for (int i = 0; i < 50; i++)
            {
                var cookie = fixture.Create<string>();
                var xmlCookie = $"<cookie pagenumber='2' pagingcookie='{cookie}'/>";
                var value = fixture.CreateMany<StubResponseJson>(rnd.Next(1, 15)).ToFlatArray();
                var success = new DataverseFetchXmlOutJson<StubResponseJson>
                {
                    Value = value,
                    PagingCookie = xmlCookie
                };

                var expected = new DataverseFetchXmlOut<StubResponseJson>(value, cookie)
                {
                    MoreRecords = true
                };

                data.Add(success, expected);
            }

            var nullCookieValue = fixture.CreateMany<StubResponseJson>(rnd.Next(1, 15)).ToFlatArray();

            var nullCookieSuccess = new DataverseFetchXmlOutJson<StubResponseJson>
            {
                Value = nullCookieValue,
                PagingCookie = null
            };

            var nullCookieExpected = new DataverseFetchXmlOut<StubResponseJson>(nullCookieValue)
            {
                MoreRecords = false
            };

            data.Add(nullCookieSuccess, nullCookieExpected);

            var emptyCookieValue = fixture.CreateMany<StubResponseJson>(rnd.Next(1, 15)).ToFlatArray();

            var emptyCookieSuccess = new DataverseFetchXmlOutJson<StubResponseJson>
            {
                Value = emptyCookieValue,
                PagingCookie = string.Empty
            };

            var emptyCookieExpected = new DataverseFetchXmlOut<StubResponseJson>(emptyCookieValue)
            {
                MoreRecords = false
            };

            data.Add(emptyCookieSuccess, emptyCookieExpected);
            return data;
        }
    }
}