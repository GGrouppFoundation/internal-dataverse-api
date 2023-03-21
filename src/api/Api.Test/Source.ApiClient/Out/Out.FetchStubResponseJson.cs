using System;
using System.Collections.Generic;
using AutoFixture;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> FetchStubResponseJsonTestData()
    {
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

            var expected = new DataverseFetchXmlOut<StubResponseJson>(value, cookie);
            
            yield return new object?[] { success, expected };
        }
    }
}