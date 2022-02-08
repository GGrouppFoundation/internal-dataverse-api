using System.Collections.Generic;
using System.Net.Http;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetStubResponseJsonOutputTestData()
    {
        yield return new object?[]
        {
            null,
            default(StubResponseJson)
        };

        yield return new object?[]
        {
            new StringContent(string.Empty),
            default(StubResponseJson)
        };

        var responseJson = new StubResponseJson
        {
            Id = 15,
            Name = "Some name"
        };

        yield return new object?[]
        {
            CreateResponseContentJson(responseJson),
            responseJson
        };
    }
}