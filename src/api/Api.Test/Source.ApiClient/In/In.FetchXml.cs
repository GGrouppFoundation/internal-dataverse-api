using System.Linq;
using System.Net;
using AutoFixture;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<DataverseFetchXmlIn, DataverseJsonRequest> FetchXmlInputTestData
    {
        get
        {
            var fixture = new Fixture();
            var annotationNullInput = fixture.Create<DataverseFetchXmlIn>() with
            {
                IncludeAnnotations = null
            };

            var inputs = fixture.CreateMany<DataverseFetchXmlIn>(50).Append(annotationNullInput).ToArray();

            var data = new TheoryData<DataverseFetchXmlIn, DataverseJsonRequest>();

            foreach (var input in inputs)
            {
                data.Add(
                    input,
                    new(
                        verb: DataverseHttpVerb.Get,
                        url: $"/api/data/v9.2/{WebUtility.UrlEncode(input.EntityPluralName)}?fetchXml={input.FetchXmlQueryString}",
                        headers: input.IncludeAnnotations switch
                        {
                            not null => [new("Prefer", $"odata.include-annotations={input.IncludeAnnotations}")],
                            _ => default
                        },
                        content: default));
            }

            return data;
        }
    }
}