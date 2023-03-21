using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoFixture;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetFetchXmlInputTestData()
    {
        var fixture = new Fixture();
        var annotationNullInput = fixture.Create<DataverseFetchXmlIn>() with
        {
            IncludeAnnotations = null
        };
            
        var inputs = fixture.CreateMany<DataverseFetchXmlIn>(50).Append(annotationNullInput).ToArray();
        var outputs = inputs.Select(Map);

        return inputs.Zip(outputs, static (i, o) => new object[] { i, o });

        static DataverseHttpRequest<Unit> Map(DataverseFetchXmlIn input)
            =>
            new(
                verb: DataverseHttpVerb.Get,
                url: $"/api/data/v9.2/{WebUtility.UrlEncode(input.EntityPluralName)}?fetchXml={input.FetchXmlQueryString}",
                headers: MapHeaders(input),
                content: default);

        static FlatArray<DataverseHttpHeader> MapHeaders(DataverseFetchXmlIn input)
            =>
            input.IncludeAnnotations is not null ? 
            new(item: new("Prefer", $"odata.include-annotations={input.IncludeAnnotations}")) : 
            default;
    }
}