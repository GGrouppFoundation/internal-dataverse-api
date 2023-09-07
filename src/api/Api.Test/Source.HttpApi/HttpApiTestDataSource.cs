using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;

namespace GarageGroup.Infra.Dataverse.Api.Test;

internal static partial class HttpApiTestDataSource
{
    private static readonly JsonSerializerOptions SerializerOptions;

    static HttpApiTestDataSource()
        =>
        SerializerOptions = new(JsonSerializerDefaults.Web);

    private static HttpMessageContent ToMessageContent(this HttpResponseMessage httpResponse)
        =>
        new(httpResponse);

    private static StringContent CreateResponseContentJson<TJson>(this TJson responseJson)
        =>
        new(
            Serialize(responseJson));

    private static StringContent ToJsonContent(this StubFailureJson failureJson)
        =>
        new(failureJson.Serialize(), default, MediaTypeNames.Application.Json);

    private static string Serialize<T>(this T value)
        =>
        JsonSerializer.Serialize(value, SerializerOptions);

    private static string GetDefaultFailureMessage(this HttpStatusCode statusCode)
        =>
        $"Dataverse respose status was {statusCode}";
}